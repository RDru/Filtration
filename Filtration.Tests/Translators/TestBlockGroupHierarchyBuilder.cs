﻿using System.Collections.Generic;
using Filtration.ObjectModel;
using Filtration.Translators;
using NUnit.Framework;

namespace Filtration.Tests.Translators
{
    [TestFixture]
    public class TestBlockGroupHierarchyBuilder
    {
        [Test]
        public void IntegrateStringListIntoBlockGroupHierarchy_ReturnsBlockGroupWithCorrectName()
        {
            // Arrange
            var inputStrings = new List<string> {"Block Group", "Sub Block Group"};

            var rootBlock = new ItemFilterBlockGroup("Root", null);
            var builder = new BlockGroupHierarchyBuilder();

            // Act
            var result = builder.IntegrateStringListIntoBlockGroupHierarchy(inputStrings, rootBlock);
            
            // Assert
            Assert.AreEqual(1, rootBlock.ChildGroups.Count);
            Assert.AreEqual("Sub Block Group", result.GroupName);
        }

        [Test]
        public void IntegrateStringListIntoBlockGroupHierarchy_SingleBlockGroup()
        {
            // Arrange
            var inputStrings = new List<string> { "Block Group" };

            var rootBlock = new ItemFilterBlockGroup("Root", null);
            var builder = new BlockGroupHierarchyBuilder();

            // Act
            var result = builder.IntegrateStringListIntoBlockGroupHierarchy(inputStrings, rootBlock);

            // Assert
            Assert.AreEqual(1, rootBlock.ChildGroups.Count);
            Assert.AreEqual("Block Group", result.GroupName);
        }

        [Test]
        public void IntegrateStringListIntoBlockGroupHierarchy_AdvancedBlockGroup_MarksBlockGroupAsAdvanced()
        {
            // Arrange
            var inputStrings = new List<string> { "~Block Group" };

            var rootBlock = new ItemFilterBlockGroup("Root", null);
            var builder = new BlockGroupHierarchyBuilder();

            // Act
            var result = builder.IntegrateStringListIntoBlockGroupHierarchy(inputStrings, rootBlock);

            // Assert
            Assert.AreEqual(1, rootBlock.ChildGroups.Count);
            Assert.AreEqual("Block Group", result.GroupName);
            Assert.AreEqual(true, result.Advanced);
        }

        [Test]
        public void IntegrateStringListIntoBlockGroupHierarchy_AdvancedBlockGroup_ChildrenAreCreatedAsAdvanced()
        {
            // Arrange
            var inputStrings = new List<string> { "~Advanced Block Group", "This should be advanced too" };

            var rootBlock = new ItemFilterBlockGroup("Root", null);
            var builder = new BlockGroupHierarchyBuilder();

            // Act
            var result = builder.IntegrateStringListIntoBlockGroupHierarchy(inputStrings, rootBlock);

            // Assert
            Assert.AreEqual(1, rootBlock.ChildGroups.Count);
            Assert.AreEqual(true, result.Advanced);
        }

        [Test]
        public void IntegrateStringListIntoBlockGroupHierarchy_ExistingAdvancedBlockGroup_SetsParentCorrectly()
        {
            // Arrange
            var inputStrings = new List<string> { "~Block Group" };

            var rootBlock = new ItemFilterBlockGroup("Root", null);
            var subBlock = new ItemFilterBlockGroup("Block Group", rootBlock, true);
            rootBlock.ChildGroups.Add(subBlock);

            var builder = new BlockGroupHierarchyBuilder();

            // Act
            var result = builder.IntegrateStringListIntoBlockGroupHierarchy(inputStrings, rootBlock);

            // Assert
            Assert.AreEqual(1, rootBlock.ChildGroups.Count);
            Assert.AreEqual("Block Group", result.GroupName);
            Assert.AreEqual(true, result.Advanced);
        }

        [Test]
        public void IntegrateStringListIntoBlockGroupHierarchy_MultipleBlockGroups()
        {
            // Arrange
            var rootBlock = new ItemFilterBlockGroup("Root", null);
            var builder = new BlockGroupHierarchyBuilder();

            // Act
            var inputStrings = new List<string> { "Block Group" };
            builder.IntegrateStringListIntoBlockGroupHierarchy(inputStrings, rootBlock);
            inputStrings = new List<string> { "Block Group 2" };
            builder.IntegrateStringListIntoBlockGroupHierarchy(inputStrings, rootBlock);
            inputStrings = new List<string> { "Block Group", "Sub Block Group" };
            var result = builder.IntegrateStringListIntoBlockGroupHierarchy(inputStrings, rootBlock);

            // Assert
            Assert.AreEqual(2, rootBlock.ChildGroups.Count);
            Assert.AreEqual("Sub Block Group", result.GroupName);
        }
    }
}

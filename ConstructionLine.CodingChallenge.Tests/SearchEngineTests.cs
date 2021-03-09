using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void Test()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red},
                Sizes = new List<Size> {Size.Small}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenSearchIsPerformed_WhenSearchOptionsIsNull_ThenItShouldThrowAnException()
        {
            var shirts = new List<Shirt>();
            var searchEngine = new SearchEngine(shirts);
            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(null));
        }

        // It can be a big result set without any filters, assuming this is allowed
        [Test]
        public void GivenSearchIsPerformed_WhenSearchOptionsSizeAndColorAreBothNull_ThenItShouldReturnAllShirts()
        {
            // arrange
            var shirts = new List<Shirt>()
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),
            };

            // act
            var searchEngine = new SearchEngine(shirts);


            var searchOptions = new SearchOptions
            {
            };

            var results = searchEngine.Search(searchOptions);

            // assert with shouldly
            results.Shirts.Count.ShouldBe(5);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenSearchIsPerformed_WhenSizeIsProvidedWithNoColorInSearchOptions_ThenItShouldReturnAllColors(){
            
            var shirts = new List<Shirt>()
            {
                new Shirt(Guid.NewGuid(), "Black - Large", Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),
            };

            var searchEngine = new SearchEngine(shirts);
            
            var searchOptions = new SearchOptions
            {
               Sizes = new List<Size>() {Size.Large }
            };

            var results = searchEngine.Search(searchOptions);

            // assert with shouldly
            results.Shirts.Count.ShouldBe(4);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenSearchIsPerformed_WhenColorIsProvidedWithNoSizeInSearchOptions_ThenItShouldReturnAllSizes()
        {

            var shirts = new List<Shirt>()
            {
                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Medium", Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color>() { Color.Blue }
            };

            var results = searchEngine.Search(searchOptions);

            // assert with shouldly
            results.Shirts.Count.ShouldBe(4);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }


        [Test]
        public void GivenSearchIsPerformed_WhenColorIsProvidedInSearchOptions_ThenItShouldReturnOnlySpecificColorSubsetShirts()
        {

            var shirts = new List<Shirt>()
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Blue - Medium", Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color>() { Color.Blue }
            };

            var results = searchEngine.Search(searchOptions);

            // assert with shouldly

            results.Shirts.Count.ShouldBe(3);
            results.Shirts.Count(x => x.Color != Color.Blue).ShouldBe(0);
            results.Shirts.Count(x => x.Color == Color.Blue).ShouldBe(3);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }


        [Test]
        public void GivenSearchIsPerformed_WhenSizeIsProvidedInSearchOptions_ThenItShouldReturnOnlySpecificSizeSubsetShirts()
        {

            var shirts = new List<Shirt>()
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size>() { Size.Large }
            };

            var results = searchEngine.Search(searchOptions);

            // assert with shouldly

            results.Shirts.Count.ShouldBe(3);
            results.Shirts.Count(x => x.Size != Size.Large).ShouldBe(0);
            results.Shirts.Count(x => x.Size == Size.Large).ShouldBe(3);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

    }

}

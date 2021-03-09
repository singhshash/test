using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }


        public SearchResults Search(SearchOptions options)
        {
            if (options == null )
            {
                throw new ArgumentNullException("Invalid options provided");
            }

            var foundShirts = _shirts.Where(shirt =>
                (!options.Sizes.Distinct().Any() || options.Sizes.Distinct().Contains(shirt.Size)) &&
                (!options.Colors.Distinct().Any() || options.Colors.Distinct().Contains(shirt.Color))).ToList();

                var sizeCounts = foundShirts
                .GroupBy(s => s.Size)
                .Select(g => new SizeCount
                {
                    Size = g.Key,
                    Count = g.Count()
                });

            var colorCounts = foundShirts
                .GroupBy(s => s.Color)
                .Select(g => new ColorCount
                {
                    Color = g.Key,
                    Count = g.Count()
                });

            var setZeroForUnfoundedSizeCountsSize = Size.All
                .Where(s => sizeCounts.All(x => x.Size != s))
                .Select(s => new SizeCount { Size = s, Count = 0 });

            var setZeroForUnfoundedColorCounts = Color.All
                .Where(c => colorCounts.All(x => x.Color != c))
                .Select(c => new ColorCount { Color = c, Count = 0 });

            return new SearchResults
            {
                Shirts = foundShirts.ToList(),
                SizeCounts = sizeCounts.Union(setZeroForUnfoundedSizeCountsSize).ToList(),
                ColorCounts = colorCounts.Union(setZeroForUnfoundedColorCounts).ToList(),
            };

        }
    }
}
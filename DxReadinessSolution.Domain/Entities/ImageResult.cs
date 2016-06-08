using System.Collections.Generic;

namespace DxReadinessSolution.Domain.Entities
{
    public class ImageResult
    {
        public List<string> Categories;
        public List<int> Ages;
        public int WomenFaces;
        public int MenFaces;
        public List<string> Tags;
        public List<Dictionary<string, float>> Emotions;

        public ImageResult()
        {
            Categories = new List<string>();
            Ages = new List<int>();
            WomenFaces = 0;
            MenFaces = 0;
            Tags = new List<string>();
            Emotions = new List<Dictionary<string, float>>();
        }


    }
}

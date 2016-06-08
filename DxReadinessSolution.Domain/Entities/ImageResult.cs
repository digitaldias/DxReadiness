using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DxReadinessSolution.Domain.Entities
{
    [DataContract]
    public class ImageResult
    {
        [DataMember]
        public List<string> Categories;

        [DataMember]
        public List<int> Ages;

        [DataMember]
        public int WomenFaces;

        [DataMember]
        public int MenFaces;

        [DataMember]
        public List<string> Tags;

        [DataMember]
        public List<Dictionary<string, float>> Emotions;

        public ImageResult()
        {
            Categories = new List<string>();
            Ages       = new List<int>();
            WomenFaces = 0;
            MenFaces   = 0;
            Tags       = new List<string>();
            Emotions   = new List<Dictionary<string, float>>();
        }


    }
}

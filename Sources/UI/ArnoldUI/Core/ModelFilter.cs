﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GoodAI.Arnold.Core
{
    public class FilterBox
    {
        public FilterBox(Vector3 position, Vector3 size)
        {
            Position = position;
            Size = size;
        }

        public Vector3 Position;
        public Vector3 Size;
    }

    public class ModelFilter
    {
        public IList<FilterBox> Boxes { get; } = new List<FilterBox>();
    }
}

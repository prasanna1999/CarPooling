using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarPooling.DataModels;

namespace CarPooling.Providers
{
    public static class AutoMapperExtensions
    {
        public static List<TDestination> MapCollectionTo<TSource, TDestination>(this List<TSource> source)
        {
            return Mapper.Map<List<TSource>, List<TDestination>>(source);
        }

        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}

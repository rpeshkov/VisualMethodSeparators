// Guids.cs
// MUST match guids.h
using System;

namespace RomanPeshkov.VisualMethodSeparators
{
    static class GuidList
    {
        public const string guidVisualMethodSeparatorsPkgString = "85d1e607-1828-4692-a148-bfe87cad338c";
        public const string guidVisualMethodSeparatorsCmdSetString = "84a6c3ea-f898-45c7-9abf-18fcf17f40a2";

        public static readonly Guid guidVisualMethodSeparatorsCmdSet = new Guid(guidVisualMethodSeparatorsCmdSetString);
    };
}
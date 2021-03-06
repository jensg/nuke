﻿// Copyright Matthias Koch 2017.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;
using Nuke.Core;
#if !NETCORE
using System.Collections;
using System.Xml.XPath;
#endif

namespace Nuke.Common.IO
{
    [PublicAPI]
    public static class XmlTasks
    {
        public static IEnumerable<string> XmlPeek (string path, string xpath)
        {
            var (elements, attributes) = GetObjects(XDocument.Load(path), xpath);
            ControlFlow.Assert(elements.Count == 0 || attributes.Count == 0, "elements.Count == 0 || attributes.Count == 0");

            return elements.Count != 0 ? elements.Select(x => x.Value) : attributes.Select(x => x.Value);
        }

        [CanBeNull]
        public static string XmlPeekSingle (string path, string xpath)
        {
            var values = XmlPeek(path, xpath).ToList();
            ControlFlow.Assert(values.Count <= 1, "values.Count <= 1");
            return values.SingleOrDefault();
        }

        public static void XmlPoke (string path, string xpath, object value)
        {
            var (elements, attributes) = GetObjects(XDocument.Load(path), xpath);

            ControlFlow.Assert((elements.Count == 1 || attributes.Count == 1) && !(elements.Count == 0 && attributes.Count == 0),
                "(elements.Count == 1 || attributes.Count == 1) && !(elements.Count == 0 && attributes.Count == 0)");

            elements.SingleOrDefault()?.SetValue(value);
            attributes.SingleOrDefault()?.SetValue(value);
        }

        private static (IReadOnlyCollection<XElement> Elements, IReadOnlyCollection<XAttribute> Attributes) GetObjects (
            XDocument document,
            string xpath)
        {
#if NETCORE
            throw new NotImplementedException();
#else
            var objects = ((IEnumerable) document.XPathEvaluate(xpath)).Cast<XObject>().ToList();
            return (objects.OfType<XElement>().ToList().AsReadOnly(),
                    objects.OfType<XAttribute>().ToList().AsReadOnly());
#endif
        }
    }
}

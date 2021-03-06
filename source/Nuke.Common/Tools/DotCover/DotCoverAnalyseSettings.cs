// Copyright Matthias Koch 2017.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;

namespace Nuke.Common.Tools.DotCover
{
    public partial class DotCoverAnalyseSettings
    {
        [NonSerialized]
        internal Action _testAction;

        public Action TestAction => _testAction;

        private string GetPackageExecutable ()
        {
            throw new NotImplementedException();
        }
    }
}

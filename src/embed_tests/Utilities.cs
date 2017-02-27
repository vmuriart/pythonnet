using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Python.EmbeddingTest
{
    /// <summary>
    /// Attribute to skip tests
    /// </summary>
    /// <remarks>
    /// If ever have to downgrade to NUnit2 again, `ITest` become `TestDetails`.
    /// https://github.com/nunit/docs/wiki/Action-Attributes
    /// http://www.nunit.org/index.php?p=actionAttributes
    /// https://www.amido.com/code/conditional-ignore-nunit-and-the-ability-to-conditionally-ignore-a-test/
    /// <para />
    /// xUnit on the other hand isn't as friendly for the cause.
    /// RavenDB implemented a couple custom classes. See if migrate to xUnit
    /// https://ayende.com/blog/166049/more-xunit-tweaks-dynamic-test-skipping
    /// https://github.com/ayende/ravendb/blob/v4.0/test/Tests.Infrastructure/SkippableFact.cs
    /// https://github.com/chrismckelt/XUnit.OptionallyIgnore
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class IgnoreIf : Attribute, ITestAction
    {
        private readonly bool _ignore;
        private readonly string _reason;

        public IgnoreIf(bool ignore, string reason)
        {
            _ignore = ignore;
            _reason = reason;
        }

        public void BeforeTest(ITest test)
        {
            if (_ignore)
            {
                Assert.Ignore(_reason);
            }
        }

        public void AfterTest(ITest test)
        {
        }

        public ActionTargets Targets { get; set; }
    }
}

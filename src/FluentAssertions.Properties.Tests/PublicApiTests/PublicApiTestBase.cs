﻿using AutoFixture;
using FluentAssertions.Properties.Tests.Common;

namespace FluentAssertions.Properties.Tests.PublicApiTests
{
    public class PublicApiTestBase
    {
        private readonly Fixture _fixture;

        public PublicApiTestBase()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new AutoFixtureNonPrefixedStringGenerator());
        }

        protected AssertReason CreateAssertReason()
        {
            return _fixture.Create<AssertReason>();
        }
    }
}

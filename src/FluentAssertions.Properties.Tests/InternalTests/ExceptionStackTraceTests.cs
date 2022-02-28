﻿using FluentAssertions.Properties.Assertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FluentAssertions.Properties.Tests.InternalTests
{
    public class ExceptionStackTraceTests
    {
        [Fact]
        public void When_exception_is_not_from_the_recognized_test_framework_exceptions_it_should_rethrow()
        {
            // Arrange
            var exception = new NullReferenceException();
            Func<object> throwingFunc = () =>
            {
                throw exception;
                return null;
            };

            // Act
            Action testAction = () => ExceptionStackTrace.StartFromCurrentFrame<object>(throwingFunc);

            // Assert
            testAction.Should().Throw<NullReferenceException>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.Xunit2;
using BackgroundPlayer.Configuration;
using Xunit;

namespace BackgroundPlayer.UnitTests.Configuration
{
    public class SkinValidator_Should
    {
        [Theory]
        [InlineAutoData(".jpg")]
        [InlineAutoData(".jpeg")]
        [InlineAutoData(".png")]
        [InlineAutoData(".bmp")]
        [InlineAutoData(".JPG")]
        [InlineAutoData(".JPEG")]
        [InlineAutoData(".PNG")]
        [InlineAutoData(".BMP")]
        public void AllowValidImageFileExtensions(string extension, SkinValidator skinValidator, string image)
        {
            var valid = skinValidator.ValidImageExtension(image + extension);
            Assert.True(valid);
        }

        [Theory]
        [InlineAutoData(".exe")]
        [InlineAutoData(".ZIP")]
        [InlineAutoData(".mp3")]
        public void FailInvalidImageFileExtension(string extension, SkinValidator skinValidator, string image)
        {
            var valid = skinValidator.ValidImageExtension(image + extension);
            Assert.False(valid);
        }

    }
}

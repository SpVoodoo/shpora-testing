using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "NegativePrecision_Throws")]
    [TestCase(0, 0, false, TestName = "ZeroPrecision_Throws")]
    [TestCase(3, -1, false, TestName = "NegativeScale_Throws")]
    [TestCase(3, 3, false, TestName = "ScaleEqualToPrecision_Throws")]
    [TestCase(3, 4, false, TestName = "ScaleGreaterThanPrecision_Throws")]
    public void Constructor_InvalidArguments_ThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive),
            $"precision={precision}, scale={scale}, onlyPositive={onlyPositive}");
    }

    [TestCase(1, 0, true, TestName = "MinimumPrecision_IsValid")]
    [TestCase(2, 1, true, TestName = "ScaleLessThanPrecision_IsValid")]
    [TestCase(3, 0, false, TestName = "IntegerFormat_IsValid")]
    public void Constructor_ValidArguments_DoesNotThrow(int precision, int scale, bool onlyPositive)
    {
        Assert.DoesNotThrow(() => new NumberValidator(precision, scale, onlyPositive),
            $"precision={precision}, scale={scale}, onlyPositive={onlyPositive}");
    }

    [TestCase("0", 17, 2, true, true, TestName = "IntegerZero_IsValid")]
    [TestCase("0.0", 17, 2, true, true, TestName = "DecimalZero_IsValid")]
    [TestCase("+1.23", 4, 2, true, true, TestName = "PositiveSign_IsValid")]
    [TestCase("-1.23", 5, 2, false, true, TestName = "NegativeNumber_IsValid")]
    [TestCase("-1.23", 5, 2, true, false, TestName = "NegativeNumber_IsInvalid")]
    [TestCase("+1.23", 3, 2, true, false, TestName = "PrecisionExceeded_IsInvalid")]
    [TestCase("0.000", 17, 2, true, false, TestName = "ScaleExceeded_IsInvalid")]
    [TestCase("00.00", 3, 2, true, false, TestName = "LeadingZerosCountTowardPrecision_IsInvalid")]
    [TestCase("1,23", 4, 2, true, true, TestName = "CommaDecimalSeparator_IsValid")]
    [TestCase("1.234", 5, 2, true, false, TestName = "TooManyFractionalDigits_IsInvalid")]
    [TestCase("123", 3, 0, true, true, TestName = "NumberDigitsEqualPrecision_IsValid")]
    [TestCase("-0", 3, 0, true, false, TestName = "ZeroWithNegativeSign_IsInvalid")]
    [TestCase("abc", 17, 2, true, false, TestName = "Letters_IsInvalid")]
    [TestCase("1.2.3", 17, 2, true, false, TestName = "MultipleSeparators_IsInvalid")]
    [TestCase("", 17, 2, true, false, TestName = "EmptyString_IsInvalid")]
    [TestCase(" ", 17, 2, true, false, TestName = "Whitespace_IsInvalid")]
    [TestCase(null, 17, 2, true, false, TestName = "Null_IsInvalid")]
    [TestCase("12.3", 3, 0, true, false, TestName = "Fraction_IsInvalid_WhenScaleIsZero")]
    public void IsValidNumber_ReturnsExpectedResult(string value, int precision, int scale, bool onlyPositive, bool expected)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        var actual = validator.IsValidNumber(value);

        Assert.That(actual, Is.EqualTo(expected),
            $"Input='{value ?? "<null>"}', precision={precision}, scale={scale}, onlyPositive={onlyPositive}");
    }
}
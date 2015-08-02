using System.ComponentModel.DataAnnotations;

namespace BookDb.Infrastructure.Validation
{
    /// <summary>
    ///     Validates ISBN 10 or 13.
    ///     Details https://www.safaribooksonline.com/library/view/regular-expressions-cookbook/9781449327453/ch04s13.html
    /// </summary>
    public class IsbnAttribute : RegularExpressionAttribute
    {
        public IsbnAttribute()
            : base(
                "^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$"
                )
        {
        }

        public override bool IsValid(object value)
        {
            // todo: check ISBN sum

            return base.IsValid(value);
        }
    }
}
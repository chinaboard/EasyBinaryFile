using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyBinaryFile
{
    internal static class Preconditions
    {
        public static void CheckNotNull<T>(T value, string name) where T : class
        {
            CheckNotNull(value, name, string.Format("{0} must not be null", name));
        }

        public static void CheckNotNull<T>(T value, string name, string message) where T : class
        {
            CheckNotBlank(name, "name", "name must not be blank");
            CheckNotBlank(message, "message", "message must not be blank");

            if (value == null)
            {
                throw new ArgumentNullException(name, message);
            }
        }

        public static void CheckNotBlank(string value, string name, string message)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name must not be blank", "name");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("message must not be blank", "message");
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckNotBlank(string value, string name)
        {
            CheckNotBlank(value, name, string.Format("{0} must not be blank", name));
        }

        public static void CheckAny<T>(IEnumerable<T> collection, string name, string message)
        {
            CheckNotBlank(name, "name", "name must not be blank");
            CheckNotBlank(message, "message", "message must not be blank");

            if (collection == null || !collection.Any())
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckTrue(bool value, string name, string message)
        {
            CheckNotBlank(name, "name", "name must not be blank");
            CheckNotBlank(message, "message", "message must not be blank");

            if (!value)
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckFalse(bool value, string name, string message)
        {
            CheckNotBlank(name, "name", "name must not be blank");
            CheckNotBlank(message, "message", "message must not be blank");

            if (value)
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckShortString(string value, string name)
        {
            CheckNotNull(value, name);
            if (value.Length > 255)
            {
                throw new ArgumentException(string.Format("Argument '{0}' must be less than or equal to 255 characters.", name));
            }
        }

        public static void CheckTypeMatches(Type expectedType, object value, string name, string message)
        {
            CheckNotBlank(name, "name", "name must not be blank");
            CheckNotBlank(message, "message", "message must not be blank");
            if (!expectedType.IsAssignableFrom(value.GetType()))
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckLess(TimeSpan value, TimeSpan maxValue, string name)
        {
            if (value < maxValue)
                return;
            throw new ArgumentOutOfRangeException(name, string.Format("Arguments {0} must be less than maxValue", name));
        }

        public static void CheckLessZero(short value, string name)
        {
            CheckNotBlank(name, "name", "name must not be blank");
            if (value < 0)
                throw new ArgumentOutOfRangeException(name, string.Format("Arguments {0} must not be less than 0", name));
        }
        public static void CheckLessZero(int value, string name)
        {
            CheckNotBlank(name, "name", "name must not be blank");
            if (value < 0)
                throw new ArgumentOutOfRangeException(name, string.Format("Arguments {0} must not be less than 0", name));
        }
        public static void CheckLessZero(long value, string name)
        {
            CheckNotBlank(name, "name", "name must not be blank");
            if (value < 0)
                throw new ArgumentOutOfRangeException(name, string.Format("Arguments {0} must not be less than 0", name));
        }
        public static void CheckLessZero(decimal value, string name)
        {
            CheckNotBlank(name, "name", "name must not be blank");
            if (value < 0)
                throw new ArgumentOutOfRangeException(name, string.Format("Arguments {0} must not be less than 0", name));
        }
        public static void CheckDisposed(bool isDisposed, string name)
        {
            if (isDisposed)
                throw new ObjectDisposedException(name);
        }
    }
}

namespace YatesSimpleRenderer.Yates
{
    using System;
    using System.Globalization;

    public struct Quaternion : IEquatable<Quaternion>
    {
        private static readonly Quaternion identityQuaternion = new Quaternion(0.0f, 0.0f, 0.0f, 1f);

        /// <summary>
        ///   <para>X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
        /// </summary>
        public float x;

        /// <summary>
        ///   <para>Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
        /// </summary>
        public float y;

        /// <summary>
        ///   <para>Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
        /// </summary>
        public float z;

        /// <summary>
        ///   <para>W component of the Quaternion. Do not directly modify quaternions.</para>
        /// </summary>
        public float w;

        public const float kEpsilon = 1E-06f;

        /// <summary>
        ///   <para>Constructs new Quaternion with given x,y,z,w components.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    case 3:
                        return this.w;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    case 3:
                        this.w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }

        /// <summary>
        ///   <para>Set x, y, z and w components of an existing Quaternion.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZ"></param>
        /// <param name="newW"></param>
        public void Set(float newX, float newY, float newZ, float newW)
        {
            this.x = newX;
            this.y = newY;
            this.z = newZ;
            this.w = newW;
        }

        /// <summary>
        ///   <para>The identity rotation (Read Only).</para>
        /// </summary>
        public static Quaternion identity
        {
            get
            {
                return Quaternion.identityQuaternion;
            }
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(
                (float)((double)lhs.w * (double)rhs.x + (double)lhs.x * (double)rhs.w + (double)lhs.y * (double)rhs.z
                        - (double)lhs.z * (double)rhs.y),
                (float)((double)lhs.w * (double)rhs.y + (double)lhs.y * (double)rhs.w + (double)lhs.z * (double)rhs.x
                        - (double)lhs.x * (double)rhs.z),
                (float)((double)lhs.w * (double)rhs.z + (double)lhs.z * (double)rhs.w + (double)lhs.x * (double)rhs.y
                        - (double)lhs.y * (double)rhs.x),
                (float)((double)lhs.w * (double)rhs.w - (double)lhs.x * (double)rhs.x - (double)lhs.y * (double)rhs.y
                        - (double)lhs.z * (double)rhs.z));
        }

        public static Vector3 operator *(Quaternion rotation, Vector3 point)
        {
            float num1 = rotation.x * 2f;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;
            float num4 = rotation.x * num1;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num9 = rotation.y * num3;
            float num10 = rotation.w * num1;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;
            Vector3 vector3;
            vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.x
                                + ((double)num7 - (double)num12) * (double)point.y
                                + ((double)num8 + (double)num11) * (double)point.z);
            vector3.y = (float)(((double)num7 + (double)num12) * (double)point.x
                                + (1.0 - ((double)num4 + (double)num6)) * (double)point.y
                                + ((double)num9 - (double)num10) * (double)point.z);
            vector3.z = (float)(((double)num8 - (double)num11) * (double)point.x
                                + ((double)num9 + (double)num10) * (double)point.y
                                + (1.0 - ((double)num4 + (double)num5)) * (double)point.z);
            return vector3;
        }

        private static bool IsEqualUsingDot(float dot)
        {
            return (double)dot > 0.999998986721039;
        }

        public static bool operator ==(Quaternion lhs, Quaternion rhs)
        {
            return Quaternion.IsEqualUsingDot(Quaternion.Dot(lhs, rhs));
        }

        public static bool operator !=(Quaternion lhs, Quaternion rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        ///   <para>The dot product between two rotations.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Dot(Quaternion a, Quaternion b)
        {
            return (float)((double)a.x * (double)b.x + (double)a.y * (double)b.y + (double)a.z * (double)b.z
                           + (double)a.w * (double)b.w);
        }

        /// <summary>
        ///   <para>Returns the angle in degrees between two rotations a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Angle(Quaternion a, Quaternion b)
        {
            float num = Quaternion.Dot(a, b);
            return !Quaternion.IsEqualUsingDot(num)
                       ? (float)((double)Math.Acos(Math.Min(Math.Abs(num), 1f)) * 2.0 * 57.2957801818848)
                       : 0.0f;
        }

        private static Vector3 Internal_MakePositive(Vector3 euler)
        {
            float num1 = -9f / (500f * (float)Math.PI);
            float num2 = 360f + num1;
            if ((double)euler.x < (double)num1)
                euler.x += 360f;
            else if ((double)euler.x > (double)num2)
                euler.x -= 360f;
            if ((double)euler.y < (double)num1)
                euler.y += 360f;
            else if ((double)euler.y > (double)num2)
                euler.y -= 360f;
            if ((double)euler.z < (double)num1)
                euler.z += 360f;
            else if ((double)euler.z > (double)num2)
                euler.z -= 360f;
            return euler;
        }

        /// <summary>
        ///   <para>Converts this quaternion to one with the same orientation but with a magnitude of 1.</para>
        /// </summary>
        /// <param name="q"></param>
        public static Quaternion Normalize(Quaternion q)
        {
            float num = (float)Math.Sqrt(Quaternion.Dot(q, q));
            if (num < Mathf.Epsilon)
                return Quaternion.identity;
            return new Quaternion(q.x / num, q.y / num, q.z / num, q.w / num);
        }

        public void Normalize()
        {
            this = Quaternion.Normalize(this);
        }

        /// <summary>
        ///   <para>Returns this quaternion with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public Quaternion normalized
        {
            get
            {
                return Quaternion.Normalize(this);
            }
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2
                   ^ this.w.GetHashCode() >> 1;
        }

        public override bool Equals(object other)
        {
            if (!(other is Quaternion))
                return false;
            return this.Equals((Quaternion)other);
        }

        public bool Equals(Quaternion other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y) && this.z.Equals(other.z) && this.w.Equals(other.w);
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string of the Quaternion.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString()
        {
            return string.Format(
                "({0:F1}, {1:F1}, {2:F1}, {3:F1})",
                (object)this.x,
                (object)this.y,
                (object)this.z,
                (object)this.w);
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string of the Quaternion.</para>
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format(
                "({0}, {1}, {2}, {3})",
                (object)this.x.ToString(format, (IFormatProvider)CultureInfo.InvariantCulture.NumberFormat),
                (object)this.y.ToString(format, (IFormatProvider)CultureInfo.InvariantCulture.NumberFormat),
                (object)this.z.ToString(format, (IFormatProvider)CultureInfo.InvariantCulture.NumberFormat),
                (object)this.w.ToString(format, (IFormatProvider)CultureInfo.InvariantCulture.NumberFormat));
        }
    }
}
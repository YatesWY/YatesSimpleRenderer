using System;

namespace YatesSimpleRenderer.Yates
{
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        private static readonly Matrix4x4 zeroMatrix = new Matrix4x4(new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
        private static readonly Matrix4x4 identityMatrix = new Matrix4x4(new Vector4(1f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 1f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1f));
        public float m00;
        public float m10;
        public float m20;
        public float m30;
        public float m01;
        public float m11;
        public float m21;
        public float m31;
        public float m02;
        public float m12;
        public float m22;
        public float m32;
        public float m03;
        public float m13;
        public float m23;
        public float m33;

        public Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
        {
            this.m00 = column0.x;
            this.m01 = column1.x;
            this.m02 = column2.x;
            this.m03 = column3.x;
            this.m10 = column0.y;
            this.m11 = column1.y;
            this.m12 = column2.y;
            this.m13 = column3.y;
            this.m20 = column0.z;
            this.m21 = column1.z;
            this.m22 = column2.z;
            this.m23 = column3.z;
            this.m30 = column0.w;
            this.m31 = column1.w;
            this.m32 = column2.w;
            this.m33 = column3.w;
        }

        public float this[int row, int column]
        {
            get
            {
                return this[row + column * 4];
            }
            set
            {
                this[row + column * 4] = value;
            }
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.m00;
                    case 1:
                        return this.m10;
                    case 2:
                        return this.m20;
                    case 3:
                        return this.m30;
                    case 4:
                        return this.m01;
                    case 5:
                        return this.m11;
                    case 6:
                        return this.m21;
                    case 7:
                        return this.m31;
                    case 8:
                        return this.m02;
                    case 9:
                        return this.m12;
                    case 10:
                        return this.m22;
                    case 11:
                        return this.m32;
                    case 12:
                        return this.m03;
                    case 13:
                        return this.m13;
                    case 14:
                        return this.m23;
                    case 15:
                        return this.m33;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.m00 = value;
                        break;
                    case 1:
                        this.m10 = value;
                        break;
                    case 2:
                        this.m20 = value;
                        break;
                    case 3:
                        this.m30 = value;
                        break;
                    case 4:
                        this.m01 = value;
                        break;
                    case 5:
                        this.m11 = value;
                        break;
                    case 6:
                        this.m21 = value;
                        break;
                    case 7:
                        this.m31 = value;
                        break;
                    case 8:
                        this.m02 = value;
                        break;
                    case 9:
                        this.m12 = value;
                        break;
                    case 10:
                        this.m22 = value;
                        break;
                    case 11:
                        this.m32 = value;
                        break;
                    case 12:
                        this.m03 = value;
                        break;
                    case 13:
                        this.m13 = value;
                        break;
                    case 14:
                        this.m23 = value;
                        break;
                    case 15:
                        this.m33 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public override int GetHashCode()
        {
            return this.GetColumn(0).GetHashCode() ^ this.GetColumn(1).GetHashCode() << 2 ^ this.GetColumn(2).GetHashCode() >> 2 ^ this.GetColumn(3).GetHashCode() >> 1;
        }

        public override bool Equals(object other)
        {
            if (!(other is Matrix4x4))
                return false;
            return this.Equals((Matrix4x4)other);
        }

        public bool Equals(Matrix4x4 other)
        {
            return this.GetColumn(0).Equals(other.GetColumn(0)) && this.GetColumn(1).Equals(other.GetColumn(1)) && this.GetColumn(2).Equals(other.GetColumn(2)) && this.GetColumn(3).Equals(other.GetColumn(3));
        }

        public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            Matrix4x4 matrix4x4;
            matrix4x4.m00 = (float)((double)lhs.m00 * (double)rhs.m00 + (double)lhs.m01 * (double)rhs.m10 + (double)lhs.m02 * (double)rhs.m20 + (double)lhs.m03 * (double)rhs.m30);
            matrix4x4.m01 = (float)((double)lhs.m00 * (double)rhs.m01 + (double)lhs.m01 * (double)rhs.m11 + (double)lhs.m02 * (double)rhs.m21 + (double)lhs.m03 * (double)rhs.m31);
            matrix4x4.m02 = (float)((double)lhs.m00 * (double)rhs.m02 + (double)lhs.m01 * (double)rhs.m12 + (double)lhs.m02 * (double)rhs.m22 + (double)lhs.m03 * (double)rhs.m32);
            matrix4x4.m03 = (float)((double)lhs.m00 * (double)rhs.m03 + (double)lhs.m01 * (double)rhs.m13 + (double)lhs.m02 * (double)rhs.m23 + (double)lhs.m03 * (double)rhs.m33);
            matrix4x4.m10 = (float)((double)lhs.m10 * (double)rhs.m00 + (double)lhs.m11 * (double)rhs.m10 + (double)lhs.m12 * (double)rhs.m20 + (double)lhs.m13 * (double)rhs.m30);
            matrix4x4.m11 = (float)((double)lhs.m10 * (double)rhs.m01 + (double)lhs.m11 * (double)rhs.m11 + (double)lhs.m12 * (double)rhs.m21 + (double)lhs.m13 * (double)rhs.m31);
            matrix4x4.m12 = (float)((double)lhs.m10 * (double)rhs.m02 + (double)lhs.m11 * (double)rhs.m12 + (double)lhs.m12 * (double)rhs.m22 + (double)lhs.m13 * (double)rhs.m32);
            matrix4x4.m13 = (float)((double)lhs.m10 * (double)rhs.m03 + (double)lhs.m11 * (double)rhs.m13 + (double)lhs.m12 * (double)rhs.m23 + (double)lhs.m13 * (double)rhs.m33);
            matrix4x4.m20 = (float)((double)lhs.m20 * (double)rhs.m00 + (double)lhs.m21 * (double)rhs.m10 + (double)lhs.m22 * (double)rhs.m20 + (double)lhs.m23 * (double)rhs.m30);
            matrix4x4.m21 = (float)((double)lhs.m20 * (double)rhs.m01 + (double)lhs.m21 * (double)rhs.m11 + (double)lhs.m22 * (double)rhs.m21 + (double)lhs.m23 * (double)rhs.m31);
            matrix4x4.m22 = (float)((double)lhs.m20 * (double)rhs.m02 + (double)lhs.m21 * (double)rhs.m12 + (double)lhs.m22 * (double)rhs.m22 + (double)lhs.m23 * (double)rhs.m32);
            matrix4x4.m23 = (float)((double)lhs.m20 * (double)rhs.m03 + (double)lhs.m21 * (double)rhs.m13 + (double)lhs.m22 * (double)rhs.m23 + (double)lhs.m23 * (double)rhs.m33);
            matrix4x4.m30 = (float)((double)lhs.m30 * (double)rhs.m00 + (double)lhs.m31 * (double)rhs.m10 + (double)lhs.m32 * (double)rhs.m20 + (double)lhs.m33 * (double)rhs.m30);
            matrix4x4.m31 = (float)((double)lhs.m30 * (double)rhs.m01 + (double)lhs.m31 * (double)rhs.m11 + (double)lhs.m32 * (double)rhs.m21 + (double)lhs.m33 * (double)rhs.m31);
            matrix4x4.m32 = (float)((double)lhs.m30 * (double)rhs.m02 + (double)lhs.m31 * (double)rhs.m12 + (double)lhs.m32 * (double)rhs.m22 + (double)lhs.m33 * (double)rhs.m32);
            matrix4x4.m33 = (float)((double)lhs.m30 * (double)rhs.m03 + (double)lhs.m31 * (double)rhs.m13 + (double)lhs.m32 * (double)rhs.m23 + (double)lhs.m33 * (double)rhs.m33);
            return matrix4x4;
        }

        public static Vector4 operator *(Matrix4x4 lhs, Vector4 vector)
        {
            Vector4 vector4;
            vector4.x = (float)((double)lhs.m00 * (double)vector.x + (double)lhs.m01 * (double)vector.y + (double)lhs.m02 * (double)vector.z + (double)lhs.m03 * (double)vector.w);
            vector4.y = (float)((double)lhs.m10 * (double)vector.x + (double)lhs.m11 * (double)vector.y + (double)lhs.m12 * (double)vector.z + (double)lhs.m13 * (double)vector.w);
            vector4.z = (float)((double)lhs.m20 * (double)vector.x + (double)lhs.m21 * (double)vector.y + (double)lhs.m22 * (double)vector.z + (double)lhs.m23 * (double)vector.w);
            vector4.w = (float)((double)lhs.m30 * (double)vector.x + (double)lhs.m31 * (double)vector.y + (double)lhs.m32 * (double)vector.z + (double)lhs.m33 * (double)vector.w);
            return vector4;
        }

        public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            return lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);
        }

        public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        ///   <para>Get a column of the matrix.</para>
        /// </summary>
        /// <param name="index"></param>
        public Vector4 GetColumn(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(this.m00, this.m10, this.m20, this.m30);
                case 1:
                    return new Vector4(this.m01, this.m11, this.m21, this.m31);
                case 2:
                    return new Vector4(this.m02, this.m12, this.m22, this.m32);
                case 3:
                    return new Vector4(this.m03, this.m13, this.m23, this.m33);
                default:
                    throw new IndexOutOfRangeException("Invalid column index!");
            }
        }

        /// <summary>
        ///   <para>Returns a row of the matrix.</para>
        /// </summary>
        /// <param name="index"></param>
        public Vector4 GetRow(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(this.m00, this.m01, this.m02, this.m03);
                case 1:
                    return new Vector4(this.m10, this.m11, this.m12, this.m13);
                case 2:
                    return new Vector4(this.m20, this.m21, this.m22, this.m23);
                case 3:
                    return new Vector4(this.m30, this.m31, this.m32, this.m33);
                default:
                    throw new IndexOutOfRangeException("Invalid row index!");
            }
        }

        /// <summary>
        ///   <para>Sets a column of the matrix.</para>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="column"></param>
        public void SetColumn(int index, Vector4 column)
        {
            this[0, index] = column.x;
            this[1, index] = column.y;
            this[2, index] = column.z;
            this[3, index] = column.w;
        }

        /// <summary>
        ///   <para>Sets a row of the matrix.</para>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="row"></param>
        public void SetRow(int index, Vector4 row)
        {
            this[index, 0] = row.x;
            this[index, 1] = row.y;
            this[index, 2] = row.z;
            this[index, 3] = row.w;
        }

        /// <summary>
        ///   <para>Transforms a position by this matrix (generic).</para>
        /// </summary>
        /// <param name="point"></param>
        public Vector3 MultiplyPoint(Vector3 point)
        {
            Vector3 vector3;
            vector3.x = (float)((double)this.m00 * (double)point.x + (double)this.m01 * (double)point.y + (double)this.m02 * (double)point.z) + this.m03;
            vector3.y = (float)((double)this.m10 * (double)point.x + (double)this.m11 * (double)point.y + (double)this.m12 * (double)point.z) + this.m13;
            vector3.z = (float)((double)this.m20 * (double)point.x + (double)this.m21 * (double)point.y + (double)this.m22 * (double)point.z) + this.m23;
            float num = 1f / ((float)((double)this.m30 * (double)point.x + (double)this.m31 * (double)point.y + (double)this.m32 * (double)point.z) + this.m33);
            vector3.x *= num;
            vector3.y *= num;
            vector3.z *= num;
            return vector3;
        }

        /// <summary>
        ///   <para>Transforms a position by this matrix (fast).</para>
        /// </summary>
        /// <param name="point"></param>
        public Vector3 MultiplyPoint3x4(Vector3 point)
        {
            Vector3 vector3;
            vector3.x = (float)((double)this.m00 * (double)point.x + (double)this.m01 * (double)point.y + (double)this.m02 * (double)point.z) + this.m03;
            vector3.y = (float)((double)this.m10 * (double)point.x + (double)this.m11 * (double)point.y + (double)this.m12 * (double)point.z) + this.m13;
            vector3.z = (float)((double)this.m20 * (double)point.x + (double)this.m21 * (double)point.y + (double)this.m22 * (double)point.z) + this.m23;
            return vector3;
        }

        /// <summary>
        ///   <para>Transforms a direction by this matrix.</para>
        /// </summary>
        /// <param name="vector"></param>
        public Vector3 MultiplyVector(Vector3 vector)
        {
            Vector3 vector3;
            vector3.x = (float)((double)this.m00 * (double)vector.x + (double)this.m01 * (double)vector.y + (double)this.m02 * (double)vector.z);
            vector3.y = (float)((double)this.m10 * (double)vector.x + (double)this.m11 * (double)vector.y + (double)this.m12 * (double)vector.z);
            vector3.z = (float)((double)this.m20 * (double)vector.x + (double)this.m21 * (double)vector.y + (double)this.m22 * (double)vector.z);
            return vector3;
        }

        /// <summary>
        ///   <para>Creates a scaling matrix.</para>
        /// </summary>
        /// <param name="vector"></param>
        public static Matrix4x4 Scale(Vector3 vector)
        {
            Matrix4x4 matrix4x4;
            matrix4x4.m00 = vector.x;
            matrix4x4.m01 = 0.0f;
            matrix4x4.m02 = 0.0f;
            matrix4x4.m03 = 0.0f;
            matrix4x4.m10 = 0.0f;
            matrix4x4.m11 = vector.y;
            matrix4x4.m12 = 0.0f;
            matrix4x4.m13 = 0.0f;
            matrix4x4.m20 = 0.0f;
            matrix4x4.m21 = 0.0f;
            matrix4x4.m22 = vector.z;
            matrix4x4.m23 = 0.0f;
            matrix4x4.m30 = 0.0f;
            matrix4x4.m31 = 0.0f;
            matrix4x4.m32 = 0.0f;
            matrix4x4.m33 = 1f;
            return matrix4x4;
        }

        /// <summary>
        ///   <para>Creates a translation matrix.</para>
        /// </summary>
        /// <param name="vector"></param>
        public static Matrix4x4 Translate(Vector3 vector)
        {
            Matrix4x4 matrix4x4;
            matrix4x4.m00 = 1f;
            matrix4x4.m01 = 0.0f;
            matrix4x4.m02 = 0.0f;
            matrix4x4.m03 = vector.x;
            matrix4x4.m10 = 0.0f;
            matrix4x4.m11 = 1f;
            matrix4x4.m12 = 0.0f;
            matrix4x4.m13 = vector.y;
            matrix4x4.m20 = 0.0f;
            matrix4x4.m21 = 0.0f;
            matrix4x4.m22 = 1f;
            matrix4x4.m23 = vector.z;
            matrix4x4.m30 = 0.0f;
            matrix4x4.m31 = 0.0f;
            matrix4x4.m32 = 0.0f;
            matrix4x4.m33 = 1f;
            return matrix4x4;
        }

        /// <summary>
        ///   <para>Creates a rotation matrix.</para>
        /// </summary>
        /// <param name="q"></param>
        public static Matrix4x4 Rotate(Quaternion q)
        {
            float num1 = q.x * 2f;
            float num2 = q.y * 2f;
            float num3 = q.z * 2f;
            float num4 = q.x * num1;
            float num5 = q.y * num2;
            float num6 = q.z * num3;
            float num7 = q.x * num2;
            float num8 = q.x * num3;
            float num9 = q.y * num3;
            float num10 = q.w * num1;
            float num11 = q.w * num2;
            float num12 = q.w * num3;
            Matrix4x4 matrix4x4;
            matrix4x4.m00 = (float)(1.0 - ((double)num5 + (double)num6));
            matrix4x4.m10 = num7 + num12;
            matrix4x4.m20 = num8 - num11;
            matrix4x4.m30 = 0.0f;
            matrix4x4.m01 = num7 - num12;
            matrix4x4.m11 = (float)(1.0 - ((double)num4 + (double)num6));
            matrix4x4.m21 = num9 + num10;
            matrix4x4.m31 = 0.0f;
            matrix4x4.m02 = num8 + num11;
            matrix4x4.m12 = num9 - num10;
            matrix4x4.m22 = (float)(1.0 - ((double)num4 + (double)num5));
            matrix4x4.m32 = 0.0f;
            matrix4x4.m03 = 0.0f;
            matrix4x4.m13 = 0.0f;
            matrix4x4.m23 = 0.0f;
            matrix4x4.m33 = 1f;
            return matrix4x4;
        }

        /// <summary>
        ///   <para>Returns a matrix with all elements set to zero (Read Only).</para>
        /// </summary>
        public static Matrix4x4 zero
        {
            get
            {
                return Matrix4x4.zeroMatrix;
            }
        }

        /// <summary>
        ///   <para>Returns the identity matrix (Read Only).</para>
        /// </summary>
        public static Matrix4x4 identity
        {
            get
            {
                return Matrix4x4.identityMatrix;
            }
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this matrix.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString()
        {
            return string.Format("{0:F5}\t{1:F5}\t{2:F5}\t{3:F5}\n{4:F5}\t{5:F5}\t{6:F5}\t{7:F5}\n{8:F5}\t{9:F5}\t{10:F5}\t{11:F5}\n{12:F5}\t{13:F5}\t{14:F5}\t{15:F5}\n", (object)this.m00, (object)this.m01, (object)this.m02, (object)this.m03, (object)this.m10, (object)this.m11, (object)this.m12, (object)this.m13, (object)this.m20, (object)this.m21, (object)this.m22, (object)this.m23, (object)this.m30, (object)this.m31, (object)this.m32, (object)this.m33);
        }
    }
}

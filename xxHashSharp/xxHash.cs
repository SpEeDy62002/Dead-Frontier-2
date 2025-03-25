using System;
using System.IO;

namespace xxHashSharp
{
	// Token: 0x02000022 RID: 34
	public class xxHash
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00007870 File Offset: 0x00005C70
		public static uint CalculateHash(byte[] buf, int len = -1, uint seed = 0U)
		{
			int i = 0;
			if (len == -1)
			{
				len = buf.Length;
			}
			uint num6;
			if (len >= 16)
			{
				int num = len - 16;
				uint num2 = seed + 2654435761U + 2246822519U;
				uint num3 = seed + 2246822519U;
				uint num4 = seed;
				uint num5 = seed - 2654435761U;
				do
				{
					num2 = xxHash.CalcSubHash(num2, buf, i);
					i += 4;
					num3 = xxHash.CalcSubHash(num3, buf, i);
					i += 4;
					num4 = xxHash.CalcSubHash(num4, buf, i);
					i += 4;
					num5 = xxHash.CalcSubHash(num5, buf, i);
					i += 4;
				}
				while (i <= num);
				num6 = xxHash.RotateLeft(num2, 1) + xxHash.RotateLeft(num3, 7) + xxHash.RotateLeft(num4, 12) + xxHash.RotateLeft(num5, 18);
			}
			else
			{
				num6 = seed + 374761393U;
			}
			num6 += (uint)len;
			while (i <= len - 4)
			{
				num6 += BitConverter.ToUInt32(buf, i) * 3266489917U;
				num6 = xxHash.RotateLeft(num6, 17) * 668265263U;
				i += 4;
			}
			while (i < len)
			{
				num6 += (uint)buf[i] * 374761393U;
				num6 = xxHash.RotateLeft(num6, 11) * 2654435761U;
				i++;
			}
			num6 ^= num6 >> 15;
			num6 *= 2246822519U;
			num6 ^= num6 >> 13;
			num6 *= 3266489917U;
			return num6 ^ (num6 >> 16);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000079B4 File Offset: 0x00005DB4
		public static uint CalculateHash(Stream stream, long len = -1L, uint seed = 0U)
		{
			int num = 0;
			if (!stream.CanRead || !stream.CanSeek)
			{
				throw new InvalidOperationException("Stream has to be seekable and readable");
			}
			if (len == -1L)
			{
				len = stream.Length;
			}
			long position = stream.Position;
			stream.Seek(0L, SeekOrigin.Begin);
			byte[] array = new byte[16];
			uint num8;
			if (len >= 16L)
			{
				long num2 = len - 16L;
				uint num3 = seed + 2654435761U + 2246822519U;
				uint num4 = seed + 2246822519U;
				uint num5 = seed;
				uint num6 = seed - 2654435761U;
				do
				{
					int num7 = 0;
					stream.Read(array, 0, array.Length);
					num3 = xxHash.CalcSubHash(num3, array, num7);
					num7 += 4;
					num4 = xxHash.CalcSubHash(num4, array, num7);
					num7 += 4;
					num5 = xxHash.CalcSubHash(num5, array, num7);
					num7 += 4;
					num6 = xxHash.CalcSubHash(num6, array, num7);
					num7 += 4;
					num += num7;
				}
				while ((long)num <= num2);
				num8 = xxHash.RotateLeft(num3, 1) + xxHash.RotateLeft(num4, 7) + xxHash.RotateLeft(num5, 12) + xxHash.RotateLeft(num6, 18);
			}
			else
			{
				num8 = seed + 374761393U;
			}
			num8 += (uint)len;
			array = new byte[4];
			while ((long)num <= len - 4L)
			{
				stream.Read(array, 0, array.Length);
				num8 += BitConverter.ToUInt32(array, 0) * 3266489917U;
				num8 = xxHash.RotateLeft(num8, 17) * 668265263U;
				num += 4;
			}
			array = new byte[1];
			while ((long)num < len)
			{
				stream.Read(array, 0, array.Length);
				num8 += (uint)array[0] * 374761393U;
				num8 = xxHash.RotateLeft(num8, 11) * 2654435761U;
				num++;
			}
			stream.Seek(position, SeekOrigin.Begin);
			num8 ^= num8 >> 15;
			num8 *= 2246822519U;
			num8 ^= num8 >> 13;
			num8 *= 3266489917U;
			return num8 ^ (num8 >> 16);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00007B94 File Offset: 0x00005F94
		public void Init(uint seed = 0U)
		{
			this._state.seed = seed;
			this._state.v1 = seed + 2654435761U + 2246822519U;
			this._state.v2 = seed + 2246822519U;
			this._state.v3 = seed;
			this._state.v4 = seed - 2654435761U;
			this._state.total_len = 0UL;
			this._state.memsize = 0;
			this._state.memory = new byte[16];
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007C20 File Offset: 0x00006020
		public bool Update(byte[] input, int len)
		{
			int num = 0;
			this._state.total_len = this._state.total_len + (ulong)len;
			if (this._state.memsize + len < 16)
			{
				Array.Copy(input, 0, this._state.memory, this._state.memsize, len);
				this._state.memsize = this._state.memsize + len;
				return true;
			}
			if (this._state.memsize > 0)
			{
				Array.Copy(input, 0, this._state.memory, this._state.memsize, 16 - this._state.memsize);
				this._state.v1 = xxHash.CalcSubHash(this._state.v1, this._state.memory, num);
				num += 4;
				this._state.v2 = xxHash.CalcSubHash(this._state.v2, this._state.memory, num);
				num += 4;
				this._state.v3 = xxHash.CalcSubHash(this._state.v3, this._state.memory, num);
				num += 4;
				this._state.v4 = xxHash.CalcSubHash(this._state.v4, this._state.memory, num);
				num += 4;
				num = 0;
				this._state.memsize = 0;
			}
			if (num <= len - 16)
			{
				int num2 = len - 16;
				uint num3 = this._state.v1;
				uint num4 = this._state.v2;
				uint num5 = this._state.v3;
				uint num6 = this._state.v4;
				do
				{
					num3 = xxHash.CalcSubHash(num3, input, num);
					num += 4;
					num4 = xxHash.CalcSubHash(num4, input, num);
					num += 4;
					num5 = xxHash.CalcSubHash(num5, input, num);
					num += 4;
					num6 = xxHash.CalcSubHash(num6, input, num);
					num += 4;
				}
				while (num <= num2);
				this._state.v1 = num3;
				this._state.v2 = num4;
				this._state.v3 = num5;
				this._state.v4 = num6;
			}
			if (num < len)
			{
				Array.Copy(input, num, this._state.memory, 0, len - num);
				this._state.memsize = len - num;
			}
			return true;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00007E60 File Offset: 0x00006260
		public uint Digest()
		{
			int i = 0;
			uint num;
			if (this._state.total_len >= 16UL)
			{
				num = xxHash.RotateLeft(this._state.v1, 1) + xxHash.RotateLeft(this._state.v2, 7) + xxHash.RotateLeft(this._state.v3, 12) + xxHash.RotateLeft(this._state.v4, 18);
			}
			else
			{
				num = this._state.seed + 374761393U;
			}
			num += (uint)this._state.total_len;
			while (i <= this._state.memsize - 4)
			{
				num += BitConverter.ToUInt32(this._state.memory, i) * 3266489917U;
				num = xxHash.RotateLeft(num, 17) * 668265263U;
				i += 4;
			}
			while (i < this._state.memsize)
			{
				num += (uint)this._state.memory[i] * 374761393U;
				num = xxHash.RotateLeft(num, 11) * 2654435761U;
				i++;
			}
			num ^= num >> 15;
			num *= 2246822519U;
			num ^= num >> 13;
			num *= 3266489917U;
			return num ^ (num >> 16);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007F9C File Offset: 0x0000639C
		private static uint CalcSubHash(uint value, byte[] buf, int index)
		{
			uint num = BitConverter.ToUInt32(buf, index);
			value += num * 2246822519U;
			value = xxHash.RotateLeft(value, 13);
			value *= 2654435761U;
			return value;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007FD0 File Offset: 0x000063D0
		private static uint RotateLeft(uint value, int count)
		{
			return (value << count) | (value >> 32 - count);
		}

		// Token: 0x0400008C RID: 140
		private const uint PRIME32_1 = 2654435761U;

		// Token: 0x0400008D RID: 141
		private const uint PRIME32_2 = 2246822519U;

		// Token: 0x0400008E RID: 142
		private const uint PRIME32_3 = 3266489917U;

		// Token: 0x0400008F RID: 143
		private const uint PRIME32_4 = 668265263U;

		// Token: 0x04000090 RID: 144
		private const uint PRIME32_5 = 374761393U;

		// Token: 0x04000091 RID: 145
		protected xxHash.XXH_State _state;

		// Token: 0x02000023 RID: 35
		public struct XXH_State
		{
			// Token: 0x04000092 RID: 146
			public ulong total_len;

			// Token: 0x04000093 RID: 147
			public uint seed;

			// Token: 0x04000094 RID: 148
			public uint v1;

			// Token: 0x04000095 RID: 149
			public uint v2;

			// Token: 0x04000096 RID: 150
			public uint v3;

			// Token: 0x04000097 RID: 151
			public uint v4;

			// Token: 0x04000098 RID: 152
			public int memsize;

			// Token: 0x04000099 RID: 153
			public byte[] memory;
		}
	}
}

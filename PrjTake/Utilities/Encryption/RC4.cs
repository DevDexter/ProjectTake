using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrjTake.Utilities.Encryption
{
    public class RC4
    {
   /*    private uint i = 0;
        private uint j = 0;
        private uint[] Arr;

        public RC4()
        {
            this.Arr = new uint[0x0100];
        }

        public void init(byte[] _arg1)
        {
            var k = _arg1.Length;
            this.i = 0;

            while (this.i < 0x0100)
            {
                this.Arr[this.i] = this.i;
                this.i++;
            };

            this.j = 0;
            this.i = 0;

            while (this.i < 0x0100)
            {
                this.j = (((this.j + this.Arr[this.i]) + _arg1[(this.i % k)]) % 0x0100);
                this.IandJ(this.i, this.j);
                this.i++;
            };

            this.i = 0;
            this.j = 0;
        }

        public void IDK(RC4 _arg1)
                {
            var k = _arg1;
            var As = (from i in k.Arr select i);
            this.Arr = k.Arr.Concat(As).ToArray();
            this.i = k.i;
            this.j = k.j;
        }

              public byte[] ZZ4(byte[] _arg1, Boolean b = false)
              {
            var a = 0;
            var k = new byte[]{};
            _arg1.Pos = 0;
            while (_arg1.bytesAvailable) {
                this.i = ((this.i + 1) % 0x0100);
                this.j = ((this.j + this._-4tg[this.i]) % 0x0100);
                this._-lz(this.i, this.j);
                if (_arg2){
                    this._-1yM(this._-4tg, this.i, this.j);
                };
                k = ((this._-4tg[this.i] + this._-4tg[this.j]) % 0x0100);
                k.writeByte((this._-4tg[k] ^ _arg1.readByte()));
            };
            k.position = 0;
            return (k);
        }*/
    }
}

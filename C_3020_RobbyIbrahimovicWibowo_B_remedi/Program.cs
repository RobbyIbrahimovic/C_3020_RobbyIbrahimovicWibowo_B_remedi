using System;
using System.Collections.Generic;

namespace MafiaGalon
{
    public class Agen
    {
        public string idAgen;
        public string namaAgen;
        public int stokAgen;

        public Agen(string idAgen, string namaAgen, int stokAgen)
        {
            this.idAgen = idAgen;
            this.namaAgen = namaAgen;
            this.stokAgen = stokAgen;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"ID Agen: {idAgen} | Nama: {namaAgen} | Stok Agen: {stokAgen} galon");
        }
    }

    public abstract class ProdukGalon
    {
        private string idProduk;
        private string namaProduk;
        private int stokGudang;

        public string IdProduk
        {
            get { return idProduk; }
            set { idProduk = value; }
        }

        public string NamaProduk
        {
            get { return namaProduk; }
            set { namaProduk = value; }
        }

        public int StokGudang
        {
            get { return stokGudang; }
            private set
            {
                if (value < 0)
                {
                    stokGudang = 0;
                }
                else
                {
                    stokGudang = value;
                }
            }
        }

        public ProdukGalon(string idProduk, string namaProduk, int stokGudang)
        {
            this.idProduk = idProduk;
            this.namaProduk = namaProduk;
            if (stokGudang < 0)
            {
                this.stokGudang = 0;
            }
            else
            {
                this.stokGudang = stokGudang;
            }
        }

        public void KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= stokGudang)
            {
                stokGudang -= jumlah;
            }
            else
            {
                Console.WriteLine("Gagal kurangi stok: jumlah gak valid atau melebihi stok gudang.");
            }
        }

        public abstract int HitungBiaya(int jumlah);

        public virtual void TampilInfoProduk()
        {
            Console.WriteLine($"ID Produk: {IdProduk} | Nama: {NamaProduk} | Stok Gudang: {StokGudang} galon");
        }
    }

    public class GalonAkua : ProdukGalon
    {
        public GalonAkua(string idProduk, string namaProduk, int stokGudang)
            : base(idProduk, namaProduk, stokGudang)
        {
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }

    public class GalonBerkeran : ProdukGalon
    {
        public string jenisKeran;

        public GalonBerkeran(string idProduk, string namaProduk, int stokGudang, string jenisKeran)
            : base(idProduk, namaProduk, stokGudang)
        {
            this.jenisKeran = jenisKeran;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 12000;
        }

        public override void TampilInfoProduk()
        {
            Console.WriteLine($"ID Produk: {IdProduk} | Nama: {NamaProduk} (Keran: {jenisKeran}) | Stok Gudang: {StokGudang} galon");
        }
    }

    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    public class DistribusiGalon : DapatDidistribusikan
    {
        public Agen agenTujuan;
        public ProdukGalon produkGalon;
        public int jumlah;

        public DistribusiGalon(Agen agenTujuan, ProdukGalon produkGalon, int jumlah)
        {
            this.agenTujuan = agenTujuan;
            this.produkGalon = produkGalon;
            this.jumlah = jumlah;
        }

        public void KirimPaket()
        {
            Console.WriteLine($"\n>> Proses kirim {jumlah} {produkGalon.NamaProduk} ke {agenTujuan.namaAgen}...");

            if (jumlah <= 0)
            {
                Console.WriteLine("Pengiriman gagal: jumlah galon harus lebih dari 0.");
                return;
            }

            if (jumlah > produkGalon.StokGudang)
            {
                Console.WriteLine($"Pengiriman gagal: stok gudang gak cukup (sisa {produkGalon.StokGudang} galon).");
                return;
            }

            produkGalon.KurangiStok(jumlah);
            agenTujuan.stokAgen += jumlah;

            int totalBiaya = produkGalon.HitungBiaya(jumlah);
            Console.WriteLine($"Paket berhasil dikirimkan ke agen {agenTujuan.namaAgen} sebanyak {jumlah} {produkGalon.NamaProduk}. Total biaya: Rp{totalBiaya}.");
        }
    }

    // Hubungan antara class SindikatGalon dan class Agen adalah Agregasi (relasi "has-a").
    // Soalnya objek Agen dibikin di luar class SindikatGalon (di Main), terus baru dimasukin ke List.
    // Jadi kalau objek SindikatGalon dihapus, objek Agen-nya tetap ada dan gak ikut kehapus.
    public class SindikatGalon
    {
        private List<Agen> daftarAgen = new List<Agen>();
        private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

        public void TambahAgen(Agen agen)
        {
            daftarAgen.Add(agen);
            Console.WriteLine($"Agen {agen.namaAgen} berhasil direkrut.");
        }

        public void TambahProduk(ProdukGalon produk)
        {
            daftarProduk.Add(produk);
            Console.WriteLine($"Produk {produk.NamaProduk} masuk ke gudang pusat.");
        }

        public void TampilkanSindikat()
        {
            Console.WriteLine("\n=== DATA SINDIKAT MAFIA GALON ===");
            Console.WriteLine("Daftar Agen:");
            foreach (Agen agen in daftarAgen)
            {
                agen.TampilInfo();
            }

            Console.WriteLine("\nDaftar Stok Gudang:");
            foreach (ProdukGalon produk in daftarProduk)
            {
                produk.TampilInfoProduk();
            }
            Console.WriteLine("=================================\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Agen agen1 = new Agen("AG01", "Markas Utara", 10);
            Agen agen2 = new Agen("AG02", "Markas Selatan", 5);

            ProdukGalon galon1 = new GalonAkua("PR01", "Air Galon Akua", 50);
            ProdukGalon galon2 = new GalonBerkeran("PR02", "Air Galon Berkeran Impor", 30, "Keran Pencet");

            SindikatGalon sindikat = new SindikatGalon();
            sindikat.TambahAgen(agen1);
            sindikat.TambahAgen(agen2);
            sindikat.TambahProduk(galon1);
            sindikat.TambahProduk(galon2);

            sindikat.TampilkanSindikat();

            Console.WriteLine("--- Cek Hitung Biaya (Polymorphism) ---");
            Console.WriteLine($"Biaya 5 {galon1.NamaProduk}: Rp{galon1.HitungBiaya(5)}");
            Console.WriteLine($"Biaya 5 {galon2.NamaProduk}: Rp{galon2.HitungBiaya(5)}");

            DapatDidistribusikan pengiriman1 = new DistribusiGalon(agen1, galon1, 15);
            pengiriman1.KirimPaket();

            DapatDidistribusikan pengiriman2 = new DistribusiGalon(agen2, galon2, 0);
            pengiriman2.KirimPaket();

            DapatDidistribusikan pengiriman3 = new DistribusiGalon(agen2, galon2, 100);
            pengiriman3.KirimPaket();

            sindikat.TampilkanSindikat();
        }
    }
}
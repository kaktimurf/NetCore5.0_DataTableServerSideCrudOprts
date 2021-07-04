using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide.Models.AuxiliaryModels
{
    ///This view model class has been referred from example created by Marien Monnier at Soft.it. All credits to Marien for this class

    /// <summary>
    /// A full result, as understood by jQuery DataTables.
    /// jQuery DataTablelerin anladığı,beklediği  sonuç.
    /// </summary>
    /// <typeparam name="T">The data type of each row.
    /// Her satırın veri türü
    /// </typeparam>
    public class DtResult<T>
    {
        /// <summary>
        /// The draw counter that this object is a response to - from the draw parameter sent as part of the data request.
        /// Note that it is strongly recommended for security reasons that you cast this parameter to an integer, rather than simply echoing back to the client what it sent in the draw parameter, in order to prevent Cross Site Scripting (XSS) attacks.
        /// 
        /// Tr=>
        /// Bu nesnenin - veri isteğinin bir parçası olarak gönderilen draw parametresinden yanıt olduğu çekme sayacı.
        /// Güvenlik nedeniyle, Siteler Arası Komut Dosyası Çalıştırma (XSS) saldırılarını önlemek için, çizim parametresinde gönderdiklerini istemciye geri yansıtmak yerine, bu parametreyi bir tam sayıya dönüştürmenizin şiddetle tavsiye edildiğini unutmayın.
        /// </summary>
        [JsonProperty("draw")]
        public int Draw { get; set; }

        /// <summary>
        /// Total records, before filtering (i.e. the total number of records in the database)
        /// Tr=>
        /// Filtrelemeden önceki toplam kayıt sayısı (veri tabanındaki toplam kayıt sayısı)
        /// </summary>
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        /// <summary>
        /// Total records, after filtering (i.e. the total number of records after filtering has been applied - not just the number of records being returned for this page of data).
        /// Tr=>
        /// Filtrelemeden sonraki toplam kayıtlar (yani, filtreleme uygulandıktan sonraki toplam kayıt sayısı - yalnızca bu veri sayfası için döndürülen kayıtların sayısı değil).
        /// </summary>
        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// The data to be displayed in the table.
        /// This is an array of data source objects, one for each row, which will be used by DataTables.
        /// Note that this parameter's name can be changed using the ajax option's dataSrc property.
        /// 
        /// Tr=>
        /// Tabloda görünen veri
        /// Bu, DataTable'lar tarafından kullanılacak her satır için bir tane olmak üzere bir dizi veri kaynağı nesnesidir.
        /// Bu parametrenin adının, ajax seçeneğinin dataSrc özelliği kullanılarak değiştirilebileceğini unutmayın.
        /// </summary>
        [JsonProperty("data")]
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Optional: If an error occurs during the running of the server-side processing script, you can inform the user of this error by passing back the error message to be displayed using this parameter.
        /// Do not include if there is no error.
        /// 
        /// Tr=>
        /// İsteğe bağlı: Sunucu tarafı işleme komut dosyasının çalışması sırasında bir hata oluşursa, bu parametreyi kullanarak görüntülenecek hata mesajını geri ileterek bu hatayı kullanıcıya bildirebilirsiniz.
        /// Hata yoksa eklemeyin.
        /// </summary>
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        public string PartialView { get; set; }
    }

    /// <summary>
    /// The additional columns that you can send to jQuery DataTables for automatic processing.
    /// Tr=>
    /// Otomatik işleme için jQuery DataTables'a gönderebileceğiniz ek sütunlar.
    /// </summary>
    public abstract class DtRow
    {
        /// <summary>
        /// Set the ID property of the dt-tag tr node to this value
        /// Tr=>
        /// dt-tag tr elementinin ID özelliğini bu özelliğe atayın
        /// </summary>
        [JsonProperty("DT_RowId")]
        public virtual string DtRowId => null;

        /// <summary>
        /// Add this class to the dt-tag tr node
        /// 
        /// Tr=>
        /// dt-tag tr elementinin class özelliğini ekle
        /// </summary>
        [JsonProperty("DT_RowClass")]
        public virtual string DtRowClass => null;

        /// <summary>
        /// Add the data contained in the object to the row using the jQuery data() method to set the data, which can also then be used for later retrieval (for example on a click event).
        /// 
        /// Tr=>
        /// Verileri ayarlamak için jQuery data() yöntemini kullanarak nesnede bulunan verileri satıra ekleyin; bu daha sonra geri almak için de kullanılabilir (örneğin bir tıklama olayında).
        /// </summary>
        [JsonProperty("DT_RowData")]
        public virtual object DtRowData => null;

        /// <summary>
        /// Add the data contained in the object to the row dt-tag tr node as attributes.
        /// The object keys are used as the attribute keys and the values as the corresponding attribute values.
        /// This is performed using using the jQuery param() method.
        /// Please note that this option requires DataTables 1.10.5 or newer.
        /// 
        /// Tr=>
        /// Nesnede bulunan verileri öznitelik olarak satır dt-tag tr elementine ekleyin
        /// Nesne anahtarları öznitelik anahtarları olarak ve değerler de karşılık gelen öznitelik değerleri olarak kullanılır.
        /// Bu, jQuery param() yöntemi kullanılarak gerçekleştirilir.
        /// Lütfen bu seçeneğin DataTable 1.10.5 veya daha yenisini gerektirdiğini unutmayın.
        /// </summary>
        [JsonProperty("DT_RowAttr")]
        public virtual object DtRowAttr => null;
    }

    /// <summary>
    /// The parameters sent by jQuery DataTables in AJAX queries.
    /// Tr=>
    /// AJAX sorgularında jQuery DataTables tarafından gönderilen parametreler.
    /// </summary>
    public class DtParameters
    {
        /// <summary>
        /// Draw counter.
        /// This is used by DataTables to ensure that the Ajax returns from server-side processing requests are drawn in sequence by DataTables (Ajax requests are asynchronous and thus can return out of sequence).
        /// This is used as part of the draw return parameter (see below).
        /// 
        /// Tr=>
        /// Draw Sayacı
        /// Bu, DataTable'lar tarafından, sunucu tarafı işleme isteklerinden gelen Ajax dönüşlerinin DataTable'lar tarafından sırayla çizilmesini sağlamak için kullanılır (Ajax istekleri eşzamansızdır ve bu nedenle sıra dışı dönebilir).
        /// Bu dödürülen parametrelerin bir parçası olarak kullanıldı
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// An array defining all columns in the table.
        /// 
        /// Tablodaki tüm sütunları tanımlayan bir dizi.
        /// </summary>
        public DtColumn[] Columns { get; set; }

        /// <summary>
        /// An array defining how many columns are being ordering upon - i.e. if the array length is 1, then a single column sort is being performed, otherwise a multi-column sort is being performed.
        /// 
        /// Tr=>
        /// Kaç sütunun sıralanacağını tanımlayan bir dizi - yani, dizi uzunluğu 1 ise, tek bir sütun sıralama gerçekleştirilir, aksi takdirde çok sütunlu bir sıralama gerçekleştirilir.
        /// </summary>
        public DtOrder[] Order { get; set; }

        /// <summary>
        /// Paging first record indicator.
        /// This is the start point in the current data set (0 index based - i.e. 0 is the first record).
        /// 
        /// Tr=>
        /// 
        ///Sayfalamanın ilk kayıt göstergesi.
        ///Bu, mevcut veri setindeki başlangıç ​​noktasıdır (0 indeks tabanlı - yani 0 ilk kayıttır)
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Number of records that the table can display in the current draw.
        /// It is expected that the number of records returned will be equal to this number, unless the server has fewer records to return.
        /// Note that this can be -1 to indicate that all records should be returned (although that negates any benefits of server-side processing!)
        /// 
        /// Tr=>
        /// 
        /// Geçerli çekilişte tablonun görüntüleyebileceği kayıt sayısı.
        /// Sunucunun döndürülecek daha az kaydı olmadığı sürece, döndürülen kayıt sayısının bu sayıya eşit olması beklenir.
        /// Tüm kayıtların döndürülmesi gerektiğini belirtmek için bunun -1 olabileceğini unutmayın (bu, sunucu tarafı işlemenin herhangi bir faydasını ortadan kaldırmasına rağmen!)    
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Global search value. To be applied to all columns which have searchable as true.
        /// Küresel arama değeri. Doğru olarak aranabilir olan tüm sütunlara uygulanır.
        /// </summary>
        public DtSearch Search { get; set; }

        /// <summary>
        /// Custom column that is used to further sort on the first Order column.
        /// İlk Sıra sütununda daha fazla sıralama yapmak için kullanılan özel sütun.
        /// </summary>
        public string SortOrder => Columns != null && Order != null && Order.Length > 0
            ? (Columns[Order[0].Column].Data +
               (Order[0].Dir == DtOrderDir.Desc ? " " + Order[0].Dir : string.Empty))
            : null;

        /// <summary>
        /// For Posting Additional Parameters to Server
        /// Sunucuya Ek Parametreler Göndermek İçin kullanılır
        /// </summary>
        public IEnumerable<string> AdditionalValues { get; set; }

    }

    /// <summary>
    /// A jQuery DataTables column.
    /// Bir JQuery DataTable kolonu
    /// </summary>
    public class DtColumn
    {
        /// <summary>
        /// Column's data source, as defined by columns.data.
        /// Kolonun veri kaynağı columns.data tarafından kontrol edilir
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Column's name, as defined by columns.name.
        /// Kolonun adı columns.name tarafından kontrol edilir
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag to indicate if this column is searchable (true) or not (false). This is controlled by columns.searchable.
        /// Bu sütunun aranabilir (true) olup olmadığını (false) belirtmek için işaretleyin. Bu, column.searchable tarafından kontrol edilir.
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        /// Flag to indicate if this column is orderable (true) or not (false). This is controlled by columns.orderable.
        /// Bu sütunun sıralanabilir (true) olup olmadığını (false) belirtmek için işaretleyin. Bu, column.orderable tarafından kontrol edilir.
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        /// Search value to apply to this specific column.
        /// Bu belirli sütuna uygulanacak arama değeri
        /// </summary>
        public DtSearch Search { get; set; }
    }

    /// <summary>
    /// An order, as sent by jQuery DataTables when doing AJAX queries.
    /// AJAX sorguları yapılırken jQuery DataTables tarafından gönderilen order .
    /// </summary>
    public class DtOrder
    {
        /// <summary>
        /// Column to which ordering should be applied.
        /// This is an index reference to the columns array of information that is also submitted to the server.
        /// 
        /// Tr=>
        /// Sıralamanın uygulanacağı sütun.
        /// Bu, aynı zamanda sunucuya gönderilen bilgilerin sütun dizisine bir dizin başvurusudur. 
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Ordering direction for this column.
        /// It will be dt-string asc or dt-string desc to indicate ascending ordering or descending ordering, respectively.
        /// 
        ///Tr=>
        ///Kolonun sıralama yönü
        ///Artan sıralamayı veya azalan sıralamayı belirtmek için dt-string asc veya dt-string desc kullanılıor.
        /// </summary>
        public DtOrderDir Dir { get; set; }
    }

    /// <summary>
    /// Sort orders of jQuery DataTables.
    /// jQuery DataTable'ların sıralama düzeni.
    /// </summary>
    public enum DtOrderDir
    {
        Asc,
        Desc
    }

    /// <summary>
    /// A search, as sent by jQuery DataTables when doing AJAX queries.
    /// AJAX sorguları yapılırken jQuery DataTables tarafından gönderilen arama şekli.
    /// </summary>
    public class DtSearch
    {
        /// <summary>
        /// Global search value. To be applied to all columns which have searchable as true.
        /// Küresel arama değeri. Doğru olarak aranabilir olan tüm sütunlara uygulanır.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// true if the global filter should be treated as a regular expression for advanced searching, false otherwise.
        /// Note that normally server-side processing scripts will not perform regular expression searching for performance reasons on large data sets, but it is technically possible and at the discretion of your script.

        /// Tr=>
        /// global filtrenin gelişmiş arama için normal bir ifade olarak ele alınması gerekiyorsa true , aksi takdirde false .
        /// Normalde sunucu tarafı işleme komut dosyalarının, büyük veri kümelerinde performans nedenleriyle düzenli ifade araması yapmayacağını unutmayın, ancak teknik olarak mümkündür ve komut dosyanızın takdirine bağlıdır.
        /// </summary>
        public bool Regex { get; set; }
    }
}

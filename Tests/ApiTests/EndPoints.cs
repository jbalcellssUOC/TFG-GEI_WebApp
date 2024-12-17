namespace Tests.ApiTests
{
    /// <summary>
    /// public class EndPoints
    /// </summary>
    public class ApiEndpoints
    {
        public string Auth_endpoint => "/v1/Authentication/Authenticate";        // Authentication endpoint

        public string Barcodes_endpoint => "/v1/Barcodes";                       // Static barcodes endpoint
        public string Products_endpoint => "/v1/Products";                       // Static barcodes endpoint

        public string Barcodes_s_getall_endpoint => "/GetStaticBarcodes";        // Get all static barcode endpoint
        public string Barcodes_s_delete_endpoint => "/DeleteStaticBarcode";      // Delete static barcode endpoint
        public string Barcodes_s_add_endpoint => "/AddStaticBarcode";            // Add static barcode endpoint
        public string Barcodes_s_update_endpoint => "/UpdateStaticBarcode";      // Update static barcode endpoint

        public string Barcodes_d_getall_endpoint => "/GetDynamicBarcodes";       // Get all dynamic barcode endpoint
        public string Barcodes_d_delete_endpoint => "/DeleteDynamicBarcode";     // Delete dynamic barcode endpoint
        public string Barcodes_d_add_endpoint => "/AddDynamicBarcode";           // Add dynamic barcode endpoint
        public string Barcodes_d_update_endpoint => "/UpdateDynamicbarcode";     // Update dynamic barcode endpoint

        public string Products_getall_endpoint => "/GetProducts";                // Get all products endpoint
        public string Products_delete_endpoint => "/DeleteProduct";              // Delete product endpoint
        public string Products_add_endpoint => "/AddProduct";                    // Add product endpoint
        public string Products_update_endpoint => "/UpdateProduct";              // Update product endpoint

    }
}

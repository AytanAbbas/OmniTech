#pragma checksum "D:\Omnitech\Omnitech\Views\Shared\_PharmacyInvoiceInfoPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7eff5173f656b1bc6bca19f3a1082267a829010a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Omnitech.Models.Shared.Views_Shared__PharmacyInvoiceInfoPartial), @"mvc.1.0.view", @"/Views/Shared/_PharmacyInvoiceInfoPartial.cshtml")]
namespace Omnitech.Models.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Omnitech\Omnitech\Views\_ViewImports.cshtml"
using Omnitech;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Omnitech\Omnitech\Views\_ViewImports.cshtml"
using Omnitech.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Omnitech\Omnitech\Views\_ViewImports.cshtml"
using Omnitech.DTOs;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Omnitech\Omnitech\Views\_ViewImports.cshtml"
using DevExtreme.AspNet.Mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Omnitech\Omnitech\Views\Shared\_PharmacyInvoiceInfoPartial.cshtml"
using Omnitech.Dtos;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Omnitech\Omnitech\Views\Shared\_PharmacyInvoiceInfoPartial.cshtml"
using Omnitech.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7eff5173f656b1bc6bca19f3a1082267a829010a", @"/Views/Shared/_PharmacyInvoiceInfoPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e93b2304b6cbb5f7f8473a509dd3ccacc8ee6836", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared__PharmacyInvoiceInfoPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PharmacyInvoiceDto>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "D:\Omnitech\Omnitech\Views\Shared\_PharmacyInvoiceInfoPartial.cshtml"
Write(Html.DevExtreme().DataGrid<PharmacyInvoiceInfo>()
    .ShowBorders(true)
    .ID("pharmacyInvoiceInfoDataContainer")
    .DataSource(Model.PharmacyInvoiceInfos, new[] { "ID" })
    .DataSource(Model.PharmacyInvoiceInfos)
    .HoverStateEnabled(true)
    .ShowBorders(true)
    .Columns(columns =>
    {
        columns.AddFor(m => m.TARIX);
        columns.AddFor(m => m.ANBAR).HeaderCellTemplate("ANBAR");
        columns.AddFor(m => m.APTEKIN_ADI).HeaderCellTemplate("APTEKIN_ADI");
        columns.AddFor(m => m.FAKTURA).HeaderCellTemplate("FAKTURA");
        columns.AddFor(m => m.SATISH).HeaderCellTemplate("SATISH");
        columns.AddFor(m => m.IADE).HeaderCellTemplate("IADE");
        columns.AddFor(m => m.NET_SATISH).HeaderCellTemplate("NET_SATISH");
        
        columns.AddFor(m => m.CAP_TARIXI).HeaderCellTemplate("CAP_TARIXI");
        columns.AddFor(m => m.KASSA_GONDERILME).HeaderCellTemplate("KASSA_GONDERILME");
        columns.AddFor(m => m.fiscal).HeaderCellTemplate("FISCAL_ID");

    })
    .Selection(s => s.Mode(SelectionMode.Single))
    .AllowColumnResizing(true)
    .Paging(p => p.PageSize(10))
    .FilterRow(f => f.Visible(true))
    .HeaderFilter(f => f.Visible(true))
    .GroupPanel(p => p.Visible(true))
    .Grouping(g => g.AutoExpandAll(false))
    .OnSelectionChanged("pharmacy_inv_info_selection_changed")
     .OnRowClick("onRowClick")
);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
#nullable restore
#line 39 "D:\Omnitech\Omnitech\Views\Shared\_PharmacyInvoiceInfoPartial.cshtml"
Write(Html.DevExtreme().ContextMenu()
    .ActiveStateEnabled(true)
    .FocusStateEnabled(true)
    .Width(200)
    .Target("#pharmacyInvoiceInfoDataContainer")
    .OnItemClick("selectPharmacyInvoiceInfo")
    .DisplayExpr("Text")
    .ItemsExpr("PharmacyInvoiceInfos")
    .DataSource(new object[] {
        new { Text = "Detallı bax",ID="selectedPharmacyInvoiceInfos" }
    })
);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n");
            WriteLiteral(@"<script>
    function onRowClick(e) {
        var kassaGonderildi = e.data.KASSA_GONDERILME === 'GÖNDƏRİLİB';
        if (kassaGonderildi) {
            Swal.fire({
                icon: 'warning',
                title: 'Bu qəbz artıq kassaya göndərilib',
              
            });
        }
    }
</script>
");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PharmacyInvoiceDto> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591

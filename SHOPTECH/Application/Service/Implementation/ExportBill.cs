using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Service.Interface;
using DinkToPdf;
using DinkToPdf.Contracts;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class ExportBill : IExportBill
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConverter _converter;

        public ExportBill(IUnitOfWork unitOfWork, IConverter converter)
        {
            _unitOfWork = unitOfWork;
            _converter = converter;
        }

        public byte[] GenerateInvoicePdf(int orderId)
        {
            var order = _unitOfWork.Orders.GetAll()
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ProductDetail)
                .FirstOrDefault(o => o.Id == orderId)
                ?? throw new Exception("Order not found");

            if (order.Status != 4)
            {
                throw new Exception("Đơn chưa hoàn thành nên không thể tạo bill");
            }

            // Tạo nội dung HTML từ danh sách sản phẩm
            string htmlContent = GenerateHtmlContent(order);

            // Cấu hình PDF
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            },
                Objects = {
                new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    HeaderSettings = { FontSize = 9, Right = "Trang [page] / [toPage]" },
                    FooterSettings = { FontSize = 9, Center = "Hóa đơn bán hàng" }
                }
            }
            };

            // Convert HTML sang PDF
            return _converter.Convert(doc);
        }

        private string GenerateHtmlContent(Order order)
        {
            // Tạo nội dung HTML từ template
            StringBuilder html = new StringBuilder();
            html.Append(@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Hóa Đơn Bán Hàng</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
            color: #333;
            line-height: 1.6;
            background-color: #f9fafb;
        }
        .invoice-box {
            max-width: 800px;
            margin: 20px auto;
            padding: 30px;
            border: 1px solid #e5e7eb;
            border-radius: 10px;
            background-color: #ffffff;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
        }
        .header {
            text-align: center;
            padding-bottom: 20px;
            margin-bottom: 30px;
            border-bottom: 2px solid #1e40af;
        }
        .header h1 {
            font-size: 28px;
            font-weight: 700;
            color: #1e40af;
            margin-bottom: 5px;
            text-transform: uppercase;
        }
        .header p {
            font-size: 14px;
            color: #6b7280;
            margin: 5px 0;
        }
        .info-section {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 30px;
            margin-bottom: 30px;
        }
        .info-section h2 {
            font-size: 16px;
            font-weight: 600;
            color: #1e40af;
            margin-bottom: 15px;
            padding-bottom: 5px;
            border-bottom: 1px solid #e5e7eb;
        }
        .info-section p {
            font-size: 14px;
            color: #4b5563;
            margin: 8px 0;
        }
        .info-section p strong {
            color: #111827;
            font-weight: 600;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 30px;
            font-size: 14px;
        }
        th {
            background-color: #1e40af;
            color: white;
            font-weight: 600;
            padding: 12px 8px;
            text-align: left;
        }
        td {
            padding: 10px 8px;
            border-bottom: 1px solid #e5e7eb;
            vertical-align: top;
        }
        tr:last-child td {
            border-bottom: none;
        }
        .text-right {
            text-align: right;
        }
        .total-row {
            font-weight: 600;
            background-color: #f3f4f6;
        }
        .total-row td {
            padding: 12px 8px;
        }
        .notes {
            padding: 15px;
            margin-bottom: 30px;
            background-color: #fef2f2;
            border-left: 4px solid #dc2626;
            font-size: 14px;
            color: #dc2626;
        }
        .signature-section {
            display: flex;
            justify-content: space-between;
            margin-top: 50px;
        }
        .signature {
            width: 45%;
            text-align: center;
        }
        .signature-title {
            font-weight: 600;
            margin-bottom: 5px;
            color: #1e40af;
        }
        .signature-instruction {
            font-size: 12px;
            color: #6b7280;
            margin-bottom: 20px;
            font-style: italic;
        }
        .signature-line {
            width: 70%;
            height: 1px;
            border-top: 1px solid #9ca3af;
            margin: 0 auto 10px;
            position: relative;
        }
        .signature-line:after {
            content: '';
            position: absolute;
            bottom: -5px;
            left: 0;
            right: 0;
            height: 10px;
            border-bottom: 1px solid #9ca3af;
        }
        .print-button {
            text-align: center;
            margin-top: 30px;
        }
        .print-button button {
            background-color: #1e40af;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 500;
            transition: background-color 0.2s;
        }
        .print-button button:hover {
            background-color: #1d4ed8;
        }
        @media print {
            body {
                background-color: white;
            }
            .invoice-box {
                box-shadow: none;
                border: none;
                padding: 0;
            }
            .no-print {
                display: none;
            }
        }
    </style>
</head>
<body>
    <div class='invoice-box'>
        <div class='header'>
            <h1>HÓA ĐƠN BÁN HÀNG</h1>
            <p>Mã hóa đơn: " + order.Id + @"</p>
            <p>Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</p>
        </div>

        <div class='info-section'>
            <div>
                <h2>Thông tin cửa hàng</h2>
                <p><strong>SHOP TECH</strong></p>
                <p>Địa chỉ: Số 298 Đ. Cầu Diễn, Minh Khai, Bắc Từ Liêm, Hà Nội</p>
                <p>SĐT: 0336361124</p>
                <p>Email: contact@shoptech.vn</p>
            </div>
            <div>
                <h2>Thông tin khách hàng</h2>
                <p><strong>Tên: " + order.Name + @"</strong></p>
                <p>Địa chỉ: " + order.Address + @"</p>
                <p>SĐT: " + order.Phone + @"</p>
            </div>
        </div>

        <table>
            <thead>
                <tr>
                    <th style='width: 5%;'>STT</th>
                    <th style='width: 40%;'>Tên sản phẩm</th>
                    <th style='width: 15%;'>Số lượng</th>
                    <th style='width: 20%;' class='text-right'>Đơn giá (VNĐ)</th>
                    <th style='width: 20%;' class='text-right'>Thành tiền (VNĐ)</th>
                </tr>
            </thead>
            <tbody>");

            // Thêm danh sách sản phẩm
            decimal total = 0;
            for (int i = 0; i < order.OrderDetails.Count(); i++)
            {
                var orderDetail = order.OrderDetails.ElementAt(i);
                decimal subTotal = orderDetail.Quantity * orderDetail.Price;
                total += subTotal;
                html.Append($@"
            <tr>
                <td>{i + 1}</td>
                <td>({orderDetail.ProductDetail.Code}) {orderDetail.ProductDetail.Name}</td>
                <td>{orderDetail.Quantity}</td>
                <td class='text-right'>{orderDetail.Price:N0}</td>
                <td class='text-right'>{subTotal:N0}</td>
            </tr>");
            }

            // Tính thuế và tổng tiền
            decimal vat = total * 0.1m;
            decimal grandTotal = total + vat;
            html.Append($@"
            </tbody>
            <tfoot>
                <tr class='total-row'>
                    <td colspan='3' rowspan='3'></td>
                    <td class='text-right'>Tổng cộng:</td>
                    <td class='text-right'>{total:N0}</td>
                </tr>
            </tfoot>
        </table>

        <div class='notes'>
            <p>Ghi chú: Hóa đơn này được xuất tự động từ hệ thống bán hàng. Quý khách vui lòng kiểm tra kỹ thông tin trước khi thanh toán.</p>
        </div>

        <div class='signature-section'>
            <div class='signature'>
                <div class='signature-title'>Người mua hàng</div>
                <div class='signature-instruction'>(Ký và ghi rõ họ tên)</div>
                <div class='signature-line'></div>
            </div>
            <div class='signature'>
                <div class='signature-title'>Người bán hàng</div>
                <div class='signature-instruction'>(Ký và ghi rõ họ tên)</div>
                <div class='signature-line'></div>
            </div>
        </div>

        <div class='print-button no-print'>
            <button onclick='window.print()'>In hóa đơn</button>
        </div>
    </div>
</body>
</html>");

            return html.ToString();
        }
    }
}

$(document).ready(function () {
    // Khởi tạo DataTable
    $('#productDetailTable').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.25/i18n/Vietnamese.json"
        }
    });

    // Xử lý khi chọn màu từ color picker
    $('#colorPicker').on('input', function () {
        const selectedColor = $(this).val();
        $('#colorCode').val(selectedColor);
        // Hiển thị trực quan màu đã chọn
        $('#colorCode').css('background-color', selectedColor);
        // Điều chỉnh màu text để dễ đọc trên nền
        const rgb = hexToRgb(selectedColor);
        const brightness = (rgb.r * 299 + rgb.g * 587 + rgb.b * 114) / 1000;
        $('#colorCode').css('color', brightness > 128 ? '#000000' : '#FFFFFF');
    });

    // Xử lý khi nhập mã màu
    $('#colorCode').on('input', function () {
        const colorValue = $(this).val();
        if (isValidColorCode(colorValue)) {
            $('#colorPicker').val(colorValue);
            // Cập nhật màu nền và màu chữ
            $(this).css('background-color', colorValue);
            const rgb = hexToRgb(colorValue);
            const brightness = (rgb.r * 299 + rgb.g * 587 + rgb.b * 114) / 1000;
            $(this).css('color', brightness > 128 ? '#000000' : '#FFFFFF');
        } else {
            // Nếu không phải mã màu hợp lệ, reset về màu mặc định
            $(this).css({
                'background-color': '',
                'color': ''
            });
        }
    });

    // Helper function để chuyển đổi hex color sang RGB
    function hexToRgb(hex) {
        // Đảm bảo hex có format đúng
        hex = hex.replace(/^#/, '');

        // Xử lý cả hex ngắn (#RGB) và hex dài (#RRGGBB)
        let r, g, b;
        if (hex.length === 3) {
            r = parseInt(hex.charAt(0) + hex.charAt(0), 16);
            g = parseInt(hex.charAt(1) + hex.charAt(1), 16);
            b = parseInt(hex.charAt(2) + hex.charAt(2), 16);
        } else {
            r = parseInt(hex.substring(0, 2), 16);
            g = parseInt(hex.substring(2, 4), 16);
            b = parseInt(hex.substring(4, 6), 16);
        }

        return { r, g, b };
    }

    // Xử lý preview hình ảnh khi chọn file
    $('#formFile').change(function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $('#imagePreview').attr('src', e.target.result);
                $('#imagePreviewContainer').show();
            }
            reader.readAsDataURL(file);
        } else {
            $('#imagePreviewContainer').hide();
        }
    });

    // Mở modal thêm mới
    $('#btnAddProductDetail').click(function () {
        resetProductDetailForm();
        $('#productDetailModalLabel').text('Thêm phiên bản sản phẩm');
        $('#productDetailModal').modal('show');
    });

    // Mở modal sửa
    $(document).on('click', '.btnEditDetail', function () {
        const id = $(this).data('id');
        resetProductDetailForm();
        $('#productDetailModalLabel').text('Cập nhật phiên bản sản phẩm');

        // Lấy thông tin chi tiết thông qua AJAX
        $.ajax({
            url: 'http://localhost:5114/api/ProductDetailAdminApi/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data) {
                    fillProductDetailForm(data);
                    $('#productDetailModal').modal('show');
                } else {
                    toastr.error('Không tìm thấy thông tin phiên bản sản phẩm');
                }
            },
            error: function (err) {
                toastr.error('Đã xảy ra lỗi khi lấy thông tin phiên bản sản phẩm');
                console.error(err);
            }
        });
    });

    // Xử lý xem chi tiết
    $(document).on('click', '.btnViewDetail', function () {
        const id = $(this).data('id');
        $.ajax({
            url: 'http://localhost:5114/api/ProductDetailAdminApi/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data) {
                    let htmlContent = `
                                <div class="row">
                                    <div class="col-md-6">
                                        <h5>Thông tin cơ bản</h5>
                                        <ul class="list-group">
                                            <li class="list-group-item"><strong>ID:</strong> ${data.id}</li>
                                            <li class="list-group-item"><strong>Tên:</strong> ${data.name}</li>
                                            <li class="list-group-item"><strong>Mã:</strong> ${data.code || 'N/A'}</li>
                                            <li class="list-group-item"><strong>Giá:</strong> ${data.price ? data.price.toLocaleString() + ' đ' : 'N/A'}</li>
                                            <li class="list-group-item"><strong>Số lượng:</strong> ${data.quantity || 'N/A'}</li>
                                            <li class="list-group-item">
                                                <strong>Trạng thái:</strong>
                                                <span class="badge ${data.isActive ? 'badge-success' : 'badge-danger'}">
                                                    ${data.isActive ? 'Hoạt động' : 'Không hoạt động'}
                                                </span>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="col-md-6">
                                        <h5>Thông tin khác</h5>
                                        <ul class="list-group">
                                            <li class="list-group-item">
                                                <strong>Màu sắc:</strong>
                                                ${data.colorName ? `
                                                <div class="d-flex align-items-center">
                                                    ${data.colorCode ? `<div style="width: 20px; height: 20px; background-color: ${data.colorCode}; margin-right: 5px; border: 1px solid #ddd;"></div>` : ''}
                                                    <span>${data.colorName}</span>
                                                </div>` : 'N/A'}
                                            </li>
                                            <li class="list-group-item">
                                                <strong>Tình trạng:</strong>
                                                <div>
                                                    ${data.isNew ? '<span class="badge badge-info mr-1">Mới</span>' : ''}
                                                    ${data.isHot ? '<span class="badge badge-danger mr-1">Hot</span>' : ''}
                                                    ${data.isSale ? '<span class="badge badge-warning mr-1">Sale</span>' : ''}
                                                    ${!data.isNew && !data.isHot && !data.isSale ? 'Thông thường' : ''}
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            `;

                    // Hiển thị hình ảnh
                    if (data.image && data.image.data) {
                        const imageSrc = `data:${data.image.type};base64,${data.image.data}`;
                        htmlContent += `
                                    <div class="text-center mt-3">
                                        <h5>Hình ảnh</h5>
                                        <img src="${imageSrc}" alt="Hình ảnh" class="img-fluid" style="max-width: 300px;">
                                    </div>
                                `;
                    }

                    // Hiển thị thuộc tính
                    if (data.productDetailAttributes && data.productDetailAttributes.length > 0) {
                        htmlContent += `
                                    <div class="mt-3">
                                        <h5>Thuộc tính sản phẩm</h5>
                                        <table class="table table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Thuộc tính</th>
                                                    <th>Giá trị</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                `;

                        data.productDetailAttributes.forEach(attr => {
                            htmlContent += `
                                        <tr>
                                            <td>${attr.productAttributeName}</td>
                                            <td>${attr.value || 'N/A'}</td>
                                        </tr>
                                    `;
                        });

                        htmlContent += `
                                            </tbody>
                                        </table>
                                    </div>
                                `;
                    }

                    $('#viewDetailContent').html(htmlContent);
                    $('#viewDetailModal').modal('show');
                } else {
                    toastr.error('Không tìm thấy thông tin phiên bản sản phẩm');
                }
            },
            error: function (err) {
                toastr.error('Đã xảy ra lỗi khi lấy thông tin phiên bản sản phẩm');
                console.error(err);
            }
        });
    });

    // Xử lý khi nhấn nút xóa
    $(document).on('click', '.btnDeleteDetail', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#deleteItemName').text(name);
        $('#btnConfirmDelete').data('id', id);
        $('#deleteConfirmModal').modal('show');
    });

    // Xử lý xác nhận xóa
    $('#btnConfirmDelete').click(function () {
        const id = $(this).data('id');

        $.ajax({
            url: 'http://localhost:5114/api/ProductDetailAdminApi/' + id,
            type: 'DELETE',
            dataType: 'json',
            success: function (result) {
                $('#deleteConfirmModal').modal('hide');
                toastr.success('Xóa phiên bản sản phẩm thành công');
                setTimeout(function () {
                        window.location.reload();
                    }, 1000);
               
            },
            error: function (err) {
                $('#deleteConfirmModal').modal('hide');
                toastr.error('Đã xảy ra lỗi khi xóa phiên bản sản phẩm');
                console.error(err);
            }
        });
    });

    // Xử lý lưu thông tin phiên bản sản phẩm
    $('#btnSaveProductDetail').click(function () {
        // Validate form
        if (!validateProductDetailForm()) {
            return;
        }

        // Tạo FormData để gửi dữ liệu form bao gồm cả file
        const formData = new FormData($('#productDetailForm')[0]);
        const productDetailId = $('#productDetailId').val();

        // Xác định URL và method dựa vào ID
        const url = productDetailId != 0
            ? 'http://localhost:5114/api/ProductDetailAdminApi'
            : 'http://localhost:5114/api/ProductDetailAdminApi';

        const method = productDetailId != 0 ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            type: method,
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                    $('#productDetailModal').modal('hide');
                    toastr.success(productDetailId != 0 ? 'Cập nhật phiên bản sản phẩm thành công' : 'Thêm phiên bản sản phẩm thành công');
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);
            },
            error: function (xhr, status, error) {
                let errorMessage = 'Đã xảy ra lỗi khi lưu phiên bản sản phẩm';

                if (xhr.responseJSON && xhr.responseJSON.errors) {
                    const errors = xhr.responseJSON.errors;
                    errorMessage = Object.keys(errors).map(key => errors[key].join(', ')).join('\n');
                }

                toastr.error(errorMessage);
                console.error(xhr, status, error);
            }
        });
    });

    // Hàm reset form
    function resetProductDetailForm() {
        $('#productDetailForm')[0].reset();
        $('#productDetailId').val(0);
        $('#imagePreviewContainer').hide();
        // Reset validation errors
        $('.is-invalid').removeClass('is-invalid');
        $('.invalid-feedback').remove();
    }

    // Hàm điền thông tin vào form
    function fillProductDetailForm(data) {
        $('#productDetailId').val(data.id);
        $('#name').val(data.name);
        $('#code').val(data.code);
        $('#price').val(data.price);
        $('#quantity').val(data.quantity);
        $('#colorName').val(data.colorName);
        $('#colorCode').val(data.colorCode);
        $('#colorPicker').val(data.colorCode);

        // Checkboxes
        $('#isActive').prop('checked', data.isActive);
        $('#isNew').prop('checked', data.isNew);
        $('#isHot').prop('checked', data.isHot);
        $('#isSale').prop('checked', data.isSale);

        // Hiển thị hình ảnh nếu có
        if (data.image && data.image.data) {
            const imageSrc = `data:${data.image.type};base64,${data.image.data}`;
            $('#imagePreview').attr('src', imageSrc);
            $('#imagePreviewContainer').show();
        }

        // Điền thông tin thuộc tính
        if (data.productDetailAttributes && data.productDetailAttributes.length > 0) {
            data.productDetailAttributes.forEach(attr => {
                $(`#attr_${attr.productAttributeId}`).val(attr.value);
            });
        }
    }

    // Hàm validate form
    function validateProductDetailForm() {
        // Reset validation errors
        $('.is-invalid').removeClass('is-invalid');
        $('.invalid-feedback').remove();

        let isValid = true;

        // Validate required fields
        const name = $('#name').val().trim();
        if (!name) {
            isValid = false;
            $('#name').addClass('is-invalid');
            $('#name').after('<div class="invalid-feedback">Vui lòng nhập tên phiên bản sản phẩm</div>');
        }

        // Validate price (nếu có)
        const price = $('#price').val();
        if (price && (isNaN(price) || parseFloat(price) < 0)) {
            isValid = false;
            $('#price').addClass('is-invalid');
            $('#price').after('<div class="invalid-feedback">Giá phải là số và không âm</div>');
        }

        // Validate quantity (nếu có)
        const quantity = $('#quantity').val();
        if (quantity && (isNaN(quantity) || parseInt(quantity) < 0)) {
            isValid = false;
            $('#quantity').addClass('is-invalid');
            $('#quantity').after('<div class="invalid-feedback">Số lượng phải là số nguyên và không âm</div>');
        }

        // Validate color code format nếu có nhập
        const colorCode = $('#colorCode').val().trim();
        if (colorCode && !isValidColorCode(colorCode)) {
            isValid = false;
            $('#colorCode').addClass('is-invalid');
            $('#colorCode').after('<div class="invalid-feedback">Mã màu không hợp lệ (ví dụ: #RRGGBB)</div>');
        }

        return isValid;
    }

    // Hàm kiểm tra mã màu hợp lệ
    function isValidColorCode(colorCode) {
        // Simple regex to check if string is a valid hex color code
        return /^#([0-9A-F]{3}){1,2}$/i.test(colorCode);
    }

    // Thêm chức năng để tự động thêm # vào mã màu nếu người dùng nhập thiếu
    $('#colorCode').on('blur', function () {
        let value = $(this).val().trim();
        if (value && !value.startsWith('#')) {
            value = '#' + value;
            $(this).val(value);

            // Cập nhật color picker
            if (isValidColorCode(value)) {
                $('#colorPicker').val(value);
                $(this).trigger('input'); // Kích hoạt sự kiện input để cập nhật màu nền
            }
        }
    });

    // Thêm nút chọn từ các mã màu phổ biến
    addCommonColorPalette();
});
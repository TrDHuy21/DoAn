// Hàm thêm bộ lọc vào URL và chuyển hướng 
function addFilter(filterName, filterValue) {
    // Lấy URL hiện tại 
    let currentUrl = new URL(window.location.href);
    let searchParams = currentUrl.searchParams;

    // Kiểm tra nếu bộ lọc đã tồn tại 
    let paramValue = searchParams.get(filterName);
    if (paramValue) {
        // Kiểm tra nếu giá trị đã tồn tại trong bộ lọc 
        let values = paramValue.split(',');
        if (!values.includes(filterValue)) {
            values.push(filterValue);
            searchParams.set(filterName, values.join(','));
        }
    } else {
        // Thêm bộ lọc mới 
        searchParams.set(filterName, filterValue);
    }

    // Chuyển hướng đến URL mới 
    window.location.href = currentUrl.toString();
}

// Hàm xóa bộ lọc khỏi URL và chuyển hướng 
function removeFilter(filterName, filterValue) {
    // Lấy URL hiện tại 
    let currentUrl = new URL(window.location.href);
    let searchParams = currentUrl.searchParams;

    // Kiểm tra nếu bộ lọc tồn tại 
    let paramValue = searchParams.get(filterName);
    if (paramValue) {
        let values = paramValue.split(',');
        // Lọc giá trị cần xóa 
        values = values.filter(v => v !== filterValue);

        if (values.length > 0) {
            // Cập nhật lại giá trị 
            searchParams.set(filterName, values.join(','));
        } else {
            // Xóa bộ lọc nếu không còn giá trị nào 
            searchParams.delete(filterName);
        }

        // Chuyển hướng đến URL mới 
        window.location.href = currentUrl.toString();
    }
}

// Hàm xử lý bộ lọc giá
function applyPriceFilter() {
    const minPrice = document.getElementById('minPrice').value;
    const maxPrice = document.getElementById('maxPrice').value;

    // Validate input
    if (minPrice && maxPrice && parseInt(minPrice) > parseInt(maxPrice)) {
        alert('Giá tối thiểu không thể lớn hơn giá tối đa!');
        return;
    }

    let currentUrl = new URL(window.location.href);
    let searchParams = currentUrl.searchParams;

    // Xóa các tham số giá cũ
    searchParams.delete('min_price');
    searchParams.delete('max_price');

    // Thêm giá mới nếu có giá trị
    if (minPrice && minPrice > 0) {
        searchParams.set('min_price', minPrice);
    }
    if (maxPrice && maxPrice > 0) {
        searchParams.set('max_price', maxPrice);
    }

    // Chuyển hướng đến URL mới
    window.location.href = currentUrl.toString();
}

// Hàm xóa bộ lọc giá
function clearPriceFilter() {
    let currentUrl = new URL(window.location.href);
    let searchParams = currentUrl.searchParams;

    // Xóa các tham số giá
    searchParams.delete('min_price');
    searchParams.delete('max_price');

    // Chuyển hướng đến URL mới
    window.location.href = currentUrl.toString();
}

// Hàm áp dụng sắp xếp
function applySorting(order, direction) {
    let currentUrl = new URL(window.location.href);
    let searchParams = currentUrl.searchParams;

    // Xóa các tham số sắp xếp cũ
    searchParams.delete('order');
    searchParams.delete('dir');

    // Thêm sắp xếp mới
    if (order && direction) {
        searchParams.set('order', order);
        searchParams.set('dir', direction);
    }

    // Chuyển hướng đến URL mới
    window.location.href = currentUrl.toString();
}

// Hàm xóa sắp xếp
function clearSorting() {
    let currentUrl = new URL(window.location.href);
    let searchParams = currentUrl.searchParams;

    // Xóa các tham số sắp xếp
    searchParams.delete('order');
    searchParams.delete('dir');

    // Chuyển hướng đến URL mới
    window.location.href = currentUrl.toString();
}

// Xử lý sự kiện khi trang tải xong
document.addEventListener('DOMContentLoaded', function () {
    // Xử lý Enter key cho input giá
    const minPriceInput = document.getElementById('minPrice');
    const maxPriceInput = document.getElementById('maxPrice');

    if (minPriceInput) {
        minPriceInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                applyPriceFilter();
            }
        });
    }

    if (maxPriceInput) {
        maxPriceInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                applyPriceFilter();
            }
        });
    }

    // Ngăn dropdown đóng khi click vào bên trong price filter
    const priceDropdown = document.querySelector('#dropdown-price + .dropdown-menu');
    if (priceDropdown) {
        priceDropdown.addEventListener('click', function (e) {
            e.stopPropagation();
        });
    }
});
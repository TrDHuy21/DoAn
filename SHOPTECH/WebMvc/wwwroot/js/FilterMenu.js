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
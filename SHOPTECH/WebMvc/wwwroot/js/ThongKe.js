<script>
    // Khai báo biến toàn cục để lưu dữ liệu
    var monthlyData = {
        labels: [],
    online: [],
    offline: []
    };

    var onlineOrdersData = {
        labels: [],
    data: []
    };

    // Khai báo các biểu đồ
    var monthlyRevenueChart, onlineOrdersChart;

    // Hàm lấy dữ liệu phần 1
    function LayDataPhan1(beginMonth, beginYear, endMonth, endYear) {
        // AJAX lấy dữ liệu doanh thu
        $.ajax({
            url: `http://localhost:5114/doanhthuvadonhang?beginMonth=${beginMonth}&beginYear=${beginYear}&endMonth=${endMonth}&endYear=${endYear}`,
            method: 'GET',
            success: function (revenueData) {
                // Tính tổng doanh thu
                const totalOnlineRevenue = revenueData.online.reduce((a, b) => a + b, 0);
                const totalOfflineRevenue = revenueData.offline.reduce((a, b) => a + b, 0);
                const totalRevenue = totalOnlineRevenue + totalOfflineRevenue;

                // Cập nhật giao diện
                $('#totalRevenue').text(formatCurrency(totalRevenue));
                $('#onlineRevenue').text(formatCurrency(totalOnlineRevenue));
                $('#offlineRevenue').text(formatCurrency(totalOfflineRevenue));

                // Cập nhật dữ liệu biểu đồ
                monthlyData = {
                    labels: revenueData.labels,
                    online: revenueData.online,
                    offline: revenueData.offline
                };

                // Cập nhật biểu đồ nếu đã khởi tạo
                if (monthlyRevenueChart) {
                    monthlyRevenueChart.data.labels = monthlyData.labels;
                    monthlyRevenueChart.data.datasets[0].data = monthlyData.online;
                    monthlyRevenueChart.data.datasets[1].data = monthlyData.offline;
                    monthlyRevenueChart.update();
                }
            },
            error: function () {
                console.error('Lỗi khi lấy dữ liệu doanh thu');
            }
        });

    // AJAX lấy dữ liệu đơn hàng
    $.ajax({
        url: `http://localhost:5114/sodonhang?beginMonth=${beginMonth}&beginYear=${beginYear}&endMonth=${endMonth}&endYear=${endYear}`,
    method: 'GET',
    success: function(ordersData) {
                // Tính tổng đơn hàng
                const totalOnlineOrders = ordersData.online.reduce((a, b) => a + b, 0);
                const totalOfflineOrders = ordersData.offline.reduce((a, b) => a + b, 0);
    const totalOrders = totalOnlineOrders + totalOfflineOrders;

    // Cập nhật giao diện
    $('#totalOrders').text(formatNumberWithCommas(totalOrders));
    $('#onlineOrders').text(formatNumberWithCommas(totalOnlineOrders));
    $('#offlineOrders').text(formatNumberWithCommas(totalOfflineOrders));

    // Cập nhật dữ liệu biểu đồ
    onlineOrdersData = {
        labels: ordersData.labels,
    data: ordersData.online
                };

    // Cập nhật biểu đồ nếu đã khởi tạo
    if(onlineOrdersChart) {
        onlineOrdersChart.data.labels = onlineOrdersData.labels;
    onlineOrdersChart.data.datasets[0].data = onlineOrdersData.data;
    onlineOrdersChart.update();
                }
            },
    error: function() {
        console.error('Lỗi khi lấy dữ liệu đơn hàng');
            }
        });
    }

    // Hàm cập nhật dữ liệu
    function updateData() {
        const startDate = $('#startDate').val();
    const endDate = $('#endDate').val();

    // Tách giá trị tháng/năm
    const [beginYear, beginMonth] = startDate.split('-').map(Number);
    const [endYear, endMonth] = endDate.split('-').map(Number);

    // Gọi hàm lấy dữ liệu
    LayDataPhan1(beginMonth, beginYear, endMonth, endYear);
    }

    // Hàm định dạng tiền tệ
    function formatCurrency(num) {
        if (num >= 1000000000) {
            return (num / 1000000000).toFixed(1) + ' tỷ';
        } else if (num >= 1000000) {
            return (num / 1000000).toFixed(0) + ' triệu';
        }
    return num.toString();
    }

    // Hàm định dạng số
    function formatNumberWithCommas(num) {
        return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    // Khởi tạo biểu đồ
    function initCharts() {
        // Biểu đồ doanh thu
        const ctx1 = document.getElementById('monthlyRevenueChart').getContext('2d');
    monthlyRevenueChart = new Chart(ctx1, {
        type: 'line',
    data: {
        labels: monthlyData.labels,
    datasets: [
    {
        label: 'Online',
    data: monthlyData.online,
    borderColor: '#2ecc71',
    backgroundColor: 'rgba(46, 204, 113, 0.2)',
    fill: true,
    tension: 0.4
                    },
    {
        label: 'Offline',
    data: monthlyData.offline,
    borderColor: '#3498db',
    backgroundColor: 'rgba(52, 152, 219, 0.2)',
    fill: true,
    tension: 0.4
                    }
    ]
            },
    options: {
        responsive: true,
    scales: {
        y: {
        beginAtZero: true,
    ticks: {
        callback: function(value) {
                                return value + ' triệu';
                            }
                        }
                    }
                }
            }
        });

    // Biểu đồ đơn hàng online
    const ctx1_5 = document.getElementById('onlineOrdersChart').getContext('2d');
    onlineOrdersChart = new Chart(ctx1_5, {
        type: 'line',
    data: {
        labels: onlineOrdersData.labels,
    atasets: [{
        label: 'Đơn hàng online',
    data: onlineOrdersData.data,
    borderColor: '#e74c3c',
    ackgroundColor: 'rgba(231, 76, 60, 0.1)',
    fill: true,
    ension: 0.4
            }]
        },
    options: {
        responsive: true,
    scales: {
        y: {
        beginAtZero: true,
    ticks: {
        callback: function(value) {
                                    return value + ' đơn';
                                }
                                }
                            }
                        }
                    }
                }
            });

        // ... (giữ nguyên các biểu đồ khác)
    }

    // Khi trang web đã tải xong
    $(document).ready(function() {
        initCharts();
    updateData(); // Gọi cập nhật dữ liệu ban đầu

        // ... (giữ nguyên các phần khác)
    });

// ... (giữ nguyên các hàm và sự kiện khác)
</script>
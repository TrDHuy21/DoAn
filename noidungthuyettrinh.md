# mở đầu
- chào hỏi, giới thiệu đề tài, lời cảm ơn
- tổng quan nội dung bài thuyết trình

- em xin chào thầy Phạm Văn Hà, cô Nguyễn Thị Hương Lan, cô Người Thị Thanh Huyền và các bạn sinh viên có mặt trong hội đồng
- em là trần đức huy, sinh viên lớp kỹ thuật thuật phần mềm k16. 
- hôm nay em xin được bảo vệ đồ án tốt nghiệp với đề tài Xây dựng website bán đồ điện tử sử dụng ASP.NET CORE WEB API và WEB MVC 
- đầu tiên em xin gửi lời cảm ơn chân thành tới thầy Đặng Trọng Hợp đã nhiệt tình hướng dẫn, giúp đỡ em trong suốt quá trình thực hiện đồ án này.
- sau đây em xin trình bày nội dung bài thuyết trình của mình, gồm

# Giới thiệu đề tài
## lý do chọn đề tài
- xu hướng bán hàng và mua hàng online
    ```
    ngày nay việc mua sắm online đã trở thành một phần không thể thiếu trong cuộc sống hàng ngày của người tiêu dùng. Với sự phát triển nhanh chóng của công nghệ và internet, việc mua sắm trực tuyến đã mang lại nhiều tiện ích cho cả người tiêu dùng và doanh nghiệp.
    ```
- Lợi ích của việc quản lý và bán hàng online
    ```
    Việc quản lý và bán hàng online giúp doanh nghiệp tiết kiệm chi phí, mở rộng thị trường và nâng cao trải nghiệm khách hàng. Người tiêu dùng cũng được hưởng lợi từ việc có nhiều lựa chọn hơn, giá cả cạnh tranh và sự tiện lợi khi mua sắm.
    ```
- xu hướng công nghệ (asp .net core, kết hợp phát triển cả api và mvc)
    ```
   ASP.NET Core là một framework phát triển ứng dụng web mạnh mẽ, hỗ trợ cả việc xây dựng API và ứng dụng MVC. Việc sử dụng ASP.NET Core giúp tăng cường hiệu suất, bảo mật và khả năng mở rộng của ứng dụng.
   Phát triển api giúp dễ dàng tích hợp ở nhiều nên tảng khác nhau, dễ dàng mở rộng hệ thống trong tương lai.
   Và bên cạnh đó, em đã sử dụng asp .net core mvc để phát triển 1 website bán hàng online.
    ```

## mục tiêu của đề tài
    - xây dựng thành công 1 hệ thống quản lý bán hàng online, cung cấp đầy đủ các chức năng cần thiết cho người quản trị và người dùng.
    - áp dụng được các công nghệ hiện đại, đảm bảo hiệu suất và bảo mật cho hệ thống.
    - nâng cao ký năng lập trình, phân tích và thiết kế hệ thống.

## nội dung nghiên cứu
    - Mô hình phát triển 1 phần mềm
    - nghiên cứu công nghệ phát triển website bằng asp .net core
    - tìm hiểu về mô hình API và MVC để phát triển website
    - tìm hiểu, phân tích, thiết kế chức năng của 1 hệ thống bán hàng online
    
# công nghệ sử dụng

## backend
- sử dụng framwork asp .net core để phát triển ứng dụng
- entity framework core để quản lý cơ sở dữ liệu
- sql server làm hệ quản trị cơ sở dữ liệu

## frontend
- html, css, javascript và boostrap để xây dựng giao diện người dùng
- bên cạnh đó sử dụng 1 số thư viên như charjs để vẽ biểu đồ, toaster để hiện thị thông báo

# phân tích và thiết kế
- về phần phân tích và thiết kế hệ thông, đầu tiên là biểu đồ use case
## biểu đồ use case
- hệ thống gồm 3 actor là khách hàng, nhân viên bán hàng và chủ cửa hàng
- khách hàng là người sử dụng hệ thống để tìm kiếm sản phẩm, mua hàng online
    - chức năng chính của khách hàng bao gồm
        - đang nhập đăng ký
        - tìm kiếm sản phẩm
        - quản lý giỏ hàng
        - đặt hàng online
        - quản lý đơn hàng 
- nhân viên bán hàng: là nhân viên bán hàng trực tiếp tại cửa hàng
    - các chức năng chính
        - đăng nhập đăng ký
        - tạo đơn hàng offline
        - quản lý đơn hàng

- chủ cửa hàng: là người chủ cửa hàng, chịu trách nhiệm quản lý toàn bộ hệ thống
    - các chức năng chính
        - đăng nhập đăng ký
        - quản lý sản phẩm
        - quản lý đơn hàng
        - quản lý người dùng
        - xem báo cáo doanh thu

## cơ sở dữ liệu
- tiếp theo là sơ đồ cơ sở dữ liệu của hệ thống
- đầu tiên là phần quản lý người dùng bảng user và được phân quyền bằng bảng role
- phần đơn hàng: 
    - có 2 loại là offline và online, được phân biệt bằng type
    - offline 
        - thì sẽ lưu thông tin nhân viên bằng trường employeeId 
        - các thuộc tính name, phone sẽ lưu thông tin khách hàng mua lúc đó
    - nếu là đơn hàng online thì sẽ lưu thông tin khách hàng bằng trường customerId
        - thì sẽ lưu thông tin khách hàng bằng trường customerId 
        - các thuộc tính name, phone sẽ lưu thông tin người nhận hàng
    - mỗi đơn hàng sẽ có nhiều chi tiết đơn hàng, được lưu trong bảng orderDetail
        - trong này em lưu id sản phẩm và giá của sản phẩm tại thời điểm đó.


- phần sản phẩm
    - mỗi sản phẩm sẽ thuộc về 1 danh mục và 1 nhãn hiệu
    - vì cửa hàng  muốn có thể bán nhiều loại sản phẩm như laptop, điện thoại, camera, tai nghe,...
    mà các loại sản phẩm khác nhau cần lưu trữ thuộc tính khác nhau nên em đã tạo thêm bảng productAttribute để lưu trữ các thuộc tính của sản phẩm của dòng sản phẩm đó
    - trong mỗi 1 sản phẩm sẽ có các phiên bản khác nhau, ví dụ như 1 chiếc điện thoại có thể có nhiều màu sắc, dung lượng khác nhau nên em đã thêm bảng productDetail và thông tin cấu hình này sẽ lưu ở bảng productDetailAttribute.

## demo
- tao 1 danh muc moi "tivi"
- tao 1 san pham moi "tivi sony x9000"
- tao 1 nhan hieu moi "sony"
- tao 1 phien ban moi "tivi sony x9000 55 inch"
- tao 1 phien ban moi "tivi sony x9000 65 inch"

- tìm kiếm sản phẩm
    - tìm theo tên
    - tìm theo danh mục, lọc
- quản lý giỏ hàng
    - thêm sản phẩm mới
    - thêm đã có
    - cập nhật số lượng: -1, lớn hơn số lượng đang có, số lượng hợp lệ
    - xóa sản phẩm
- tạo 1 đơn hàng mới online, offline
- thông báo mail

- quản lý đơn hàng online
- thay đổi trạng thái
    - thay đổi từ chờ xác nhận đến thành công
    - hủy.
- show log.

- show thống kê doanh thu


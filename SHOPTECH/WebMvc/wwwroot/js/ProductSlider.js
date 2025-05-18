document.querySelectorAll('.thumbnail').forEach((thumbnail) => {
    thumbnail.addEventListener('click', function () {
        // Xóa lớp active từ tất cả thumbnail
        document.querySelectorAll('.thumbnail').forEach((thumb) => {
            thumb.classList.remove('active-thumbnail');
        });
        // Thêm lớp active cho thumbnail đang được chọn
        this.classList.add('active-thumbnail');

        // Cuộn thumbnail vào giữa
        const container = document.querySelector('.thumbnail-container');
        const thumbnailRect = this.getBoundingClientRect();
        const containerRect = container.getBoundingClientRect();
        const scrollLeft = thumbnailRect.left - containerRect.left + container.scrollLeft - (containerRect.width / 2) + (thumbnailRect.width / 2);
        container.scrollTo({ left: scrollLeft, behavior: 'smooth' });
    });
});
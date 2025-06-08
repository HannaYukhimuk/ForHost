function initSlideNavigation() {
    const prevBtn = document.getElementById('prevSlideBtn');
    const nextBtn = document.getElementById('nextSlideBtn');
    const slidesList = document.querySelectorAll('#slidesList .list-group-item[data-slide-id]');
    
    if (!slidesList.length) return;
    
    function getCurrentSlideIndex() {
        return Array.from(slidesList).findIndex(slide => 
            slide.classList.contains('active'));
    }
    
    prevBtn?.addEventListener('click', () => {
        const currentSlideIndex = getCurrentSlideIndex();
        if (currentSlideIndex > 0) {
            loadSlide(slidesList[currentSlideIndex - 1].dataset.slideId);
        }
    });
    
    nextBtn?.addEventListener('click', () => {
        const currentSlideIndex = getCurrentSlideIndex();
        if (currentSlideIndex < slidesList.length - 1) {
            loadSlide(slidesList[currentSlideIndex + 1].dataset.slideId);
        }
    });
    
    slidesList.forEach(slide => {
        slide.addEventListener('click', function(e) {
            if (e.target.closest('.delete-slide-btn')) return;
            const slideId = this.dataset.slideId;
            loadSlide(slideId);
        });
    });
}
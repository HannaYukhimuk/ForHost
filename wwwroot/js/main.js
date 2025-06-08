document.addEventListener('DOMContentLoaded', function() {
    const currentSlideId = document.getElementById('currentSlideId').value;
    if (currentSlideId) {
        loadSlide(currentSlideId);
    }
    
    initSlideNavigation();
    initDragAndDrop();
     initSlideManagement();
    initAddTextFunctionality();
    initDeleteSlideFunctionality();
    initContextMenu();
    initAddShapeFunctionality();
    initAceptManagment();
});















function initDeleteSlideFunctionality() {
    const deleteModal = document.getElementById('deleteSlideModal');
    let currentSlideIdToDelete = null;

    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function(event) {
            const button = event.relatedTarget;
            currentSlideIdToDelete = button.getAttribute('data-slide-id');
        });
    }

    document.getElementById('confirmDeleteBtn')?.addEventListener('click', async function() {
        if (!currentSlideIdToDelete) {
            alert('Slide ID not specified');
            return;
        }

        const deleteBtn = this;
        deleteBtn.disabled = true;
        deleteBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Deleting...';

        try {
            const response = await fetch(`/api/slides/${currentSlideIdToDelete}`, {
                method: 'DELETE',
                headers: {
                    'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const modal = bootstrap.Modal.getInstance(deleteModal);
                modal?.hide();
                
                const slideElement = document.querySelector(`.list-group-item[data-slide-id="${currentSlideIdToDelete}"]`);
                if (slideElement) {
                    slideElement.remove();
                    
                    const slides = document.querySelectorAll('#slidesList .list-group-item:not(.mt-2)');
                    slides.forEach((item, index) => {
                        const numberSpan = item.querySelector('span:first-child');
                        if (numberSpan) {
                            numberSpan.textContent = `Slide ${index + 1}`;
                            item.setAttribute('data-slide-order', index + 1);
                        }
                    });
                    
                    if (document.getElementById('currentSlideId').value === currentSlideIdToDelete) {
                        const firstSlide = document.querySelector('#slidesList .list-group-item[data-slide-id]');
                        if (firstSlide) {
                            loadSlide(firstSlide.dataset.slideId);
                        } else {
                            document.getElementById('slideCanvas').innerHTML = '<p>No slides available</p>';
                        }
                    }
                }
            } else {
                const error = await response.text();
                console.error('Error:', error);
                alert('Failed to delete slide');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('An error occurred while deleting the slide');
        } finally {
            deleteBtn.disabled = false;
            deleteBtn.textContent = 'Delete';
        }
    });
}







function initAceptManagment(){
    document.querySelectorAll('.promote-btn').forEach(btn => {
    btn.addEventListener('click', function() {
        const userId = this.dataset.userId;
        const presentationId = this.dataset.presentationId;
        const requesterNickname = "@User.Identity?.Name"; 
        
        if (!confirm('Are you sure you want to promote this user to Editor?')) {
            return;
        }

        fetch(`/api/Presentations/${presentationId}/promote/${userId}?requesterNickname=${encodeURIComponent(requesterNickname)}`, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('#__RequestVerificationToken').value,
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
            if (response.ok) {
                location.reload(); 
            } else if (response.status === 403) {
                alert('Only creator can promote users');
            } else {
                alert('Failed to promote user');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred while promoting user');
        });
    });
});
}






function initSlideManagement() {
    document.getElementById('addSlideBtn')?.addEventListener('click', async function() {

        const presentationId = document.getElementById('presentationId').value;
        
        const addBtn = this;
        const originalText = addBtn.innerHTML;
        
        addBtn.disabled = true;
        addBtn.innerHTML = `
            <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
            Adding...
        `;

        try {
            const response = await fetch(`/api/slides/${presentationId}`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value,
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(await response.text() || 'Failed to add slide');
            }

            const slide = await response.json();
            
            const slidesList = document.getElementById('slidesList');
            const slideItem = document.createElement('div');
            slideItem.className = 'list-group-item d-flex justify-content-between align-items-center';
            slideItem.dataset.slideId = slide.id;
            slideItem.dataset.slideOrder = slide.order;
            
            slideItem.innerHTML = `
                <span>Slide ${slide.order}</span>
                <button class="btn btn-sm btn-outline-danger delete-slide-btn" 
                        data-bs-toggle="modal" 
                        data-bs-target="#deleteSlideModal"
                        data-slide-id="${slide.id}">
                    ×
                </button>
            `;
            
            slideItem.addEventListener('click', function() {
                loadSlide(slide.id);
            });
            
            const addButton = document.querySelector('#slidesList .mt-2');
            slidesList.insertBefore(slideItem, addButton);
            
            loadSlide(slide.id);
            
            initDeleteSlideFunctionality();
            
        } catch (error) {
            console.error('Error adding slide:', error);
            alert('Error adding slide: ' + error.message);
        } finally {
            addBtn.disabled = false;
            addBtn.innerHTML = originalText;
        }
    });
}
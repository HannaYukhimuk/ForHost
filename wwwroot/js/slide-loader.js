async function loadSlide(slideId) {
    if (!slideId) return;
    
    try {
        const slideCanvas = document.getElementById('slideCanvas');
        if (slideCanvas) {
            slideCanvas.innerHTML = '<div class="text-center mt-5"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>';
        }
        
        const response = await fetch(`/api/slides/${slideId}/elements`);
        if (!response.ok) throw new Error('Failed to load slide');
        
        const data = await response.json();
        const { elements, slide } = data;
        
        document.getElementById('currentSlideId').value = slideId;
        
        if (slideCanvas) {
            slideCanvas.innerHTML = '';
            slideCanvas.style.backgroundColor = slide.background || '#ffffff';
            
            elements.forEach(element => {
                const elementDiv = document.createElement('div');
                elementDiv.className = 'slide-element';
                elementDiv.dataset.elementId = element.id;
                elementDiv.style.cssText = `
                    position: absolute;
                    left: ${element.positionX}px;
                    top: ${element.positionY}px;
                    width: ${element.width}px;
                    height: ${element.height}px;
                    z-index: ${element.zIndex || (element.type === 'Text' ? 10 : 0)};
                `;
                
                if (element.type === 'Text') {
                    elementDiv.innerHTML = `
                        <div contenteditable="true" 
                            style="color: ${element.color};
                                   font-size: ${element.fontSize}px;
                                   font-style: ${element.fontStyle};
                                   font-weight: ${element.fontWeight};
                                   width: 100%;
                                   height: 100%;
                                   padding: 2px;
                                   box-sizing: border-box;
                                   cursor: move;
                                   white-space: normal;">
                            ${element.content}
                        </div>
                    `;
                } else if (element.type === 'Rectangle' || element.type === 'Circle' || element.type === 'Triangle') {
                    let shapeStyle = '';
                    
                    switch(element.type) {
                        case 'Circle':
                            shapeStyle = `
                                background-color: ${element.fillColor || '#ffffff'};
                                border: ${element.borderWidth || 1}px solid ${element.borderColor || '#000000'};
                                width: 100%;
                                height: 100%;
                                box-sizing: border-box;
                                border-radius: 50%;
                            `;
                            break;
                        case 'Triangle':
                            shapeStyle = `
                                width: 0;
                                height: 0;
                                background-color: ${element.fillColor || '#ffffff'};
                                border: ${element.borderWidth || 1}px solid ${element.borderColor || '#000000'};
                                border-left: ${element.width/2}px solid transparent;
                                border-right: ${element.width/2}px solid transparent;
                                border-bottom: ${element.height}px solid ${element.fillColor || '#000000'};
                                border-top: none;
                                background: none;
                                position: absolute;
                                top: 0;
                                left: 0;
                            `;
                            break;
                        default: 
                            shapeStyle = `
                                background-color: ${element.fillColor || '#ffffff'};
                                border: ${element.borderWidth || 1}px solid ${element.borderColor || '#000000'};
                                width: 100%;
                                height: 100%;
                                box-sizing: border-box;
                            `;
                    }
                    
                    elementDiv.innerHTML = `<div style="${shapeStyle}"></div>`;
                }
                
                slideCanvas.appendChild(elementDiv);
            });
            
            initDragAndDrop();
        }
        
        document.querySelectorAll('#slidesList .list-group-item').forEach(item => {
            item.classList.toggle('active', item.dataset.slideId === slideId);
        });
    } catch (error) {
        console.error('Error loading slide:', error);
        alert('Failed to load slide');
    }
}


document.querySelectorAll('.promote-btn').forEach(button => {
    button.addEventListener('click', async function () {
        const userId = this.dataset.userId;
        const presentationId = this.dataset.presentationId;

        try {
            const response = await fetch(`/api/presentations/${presentationId}/promote/${userId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.getElementById('__RequestVerificationToken')?.value || ''
                }
            });

            if (response.ok) {
                alert('User promoted to editor');
                location.reload(); 
            } else {
                const error = await response.text();
                alert('Error: ' + error);
            }
        } catch (err) {
            console.error(err);
            alert('Request failed');
        }
    });
});

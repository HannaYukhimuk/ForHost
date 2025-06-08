function initAddTextFunctionality() {
    const confirmAddTextBtn = document.getElementById('confirmAddText');
    if (!confirmAddTextBtn) return;

    confirmAddTextBtn.addEventListener('click', async function() {
        const textContent = document.getElementById('textContent');
        const textColor = document.getElementById('textColor');
        const textFontStyle = document.getElementById('textFontStyle');
        const textFontSize = document.getElementById('textFontSize');
        
        if (!textContent || !textColor || !textFontStyle || !textFontSize) {
            alert('Form not found');
            return;
        }

        const content = textContent.value.trim();
        if (!content) {
            alert('Please enter text');
            textContent.focus();
            return;
        }

        const currentSlideId = document.getElementById('currentSlideId').value;
        if (!currentSlideId) {
            alert('No slide selected');
            return;
        }

        const addBtn = this;
        const originalText = addBtn.innerHTML;
        
        addBtn.disabled = true;
        addBtn.innerHTML = `
            <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
            Adding...
        `;

        try {
            const response = await fetch('/api/elements/text', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value
                },
                body: JSON.stringify({
                    slideId: currentSlideId,
                    content: content,
                    color: textColor.value,
                    width: 200,
                    height: 50,
                    fontSize: parseInt(textFontSize.value),
                    fontStyle: textFontStyle.value.includes('italic') ? 'italic' : 'normal',
                    fontWeight: textFontStyle.value.includes('bold') ? 'bold' : 'normal'
                })
            });

            if (!response.ok) {
                throw new Error(await response.text() || 'Server error');
            }

            const element = await response.json();
            
            const slideCanvas = document.getElementById('slideCanvas');
            if (slideCanvas) {
                const textElement = document.createElement('div');
                textElement.className = 'slide-element';
                textElement.dataset.elementId = element.id;
                textElement.style.cssText = `
                    position: absolute;
                    left: ${element.positionX}px;
                    top: ${element.positionY}px;
                    width: ${element.width}px;
                    height: ${element.height}px;
                    z-index: 10;
                `;
                
                textElement.innerHTML = `
                    <div contenteditable="true" 
                         style="color: ${element.color}; 
                                font-size: ${element.fontSize}px;
                                font-style: ${element.fontStyle};
                                font-weight: ${element.fontWeight};
                                width: 100%;
                                height: 100%;
                                padding: 2px;
                                cursor: move;">
                        ${element.content}
                    </div>
                `;
                slideCanvas.appendChild(textElement);
                
                textElement.addEventListener('mousedown', startDrag);
            }
            
            const modal = bootstrap.Modal.getInstance(document.getElementById('addTextModal'));
            if (modal) {
                modal.hide();
            }
            
            textContent.value = '';
        } catch (error) {
            console.error('Error:', error);
            alert('Error adding text: ' + error.message);
        } finally {
            addBtn.disabled = false;
            addBtn.innerHTML = originalText;
        }
    });
    
    document.getElementById('addTextModal')?.addEventListener('show.bs.modal', function() {
        document.getElementById('textContent').value = '';
    });
}

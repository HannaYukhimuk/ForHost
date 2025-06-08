function initAddShapeFunctionality() {
    const confirmAddShapeBtn = document.getElementById('confirmAddShape');
    if (!confirmAddShapeBtn) return;

    confirmAddShapeBtn.addEventListener('click', async function() {
        const shapeType = document.getElementById('shapeType').value;
        const shapeFillColor = document.getElementById('shapeFillColor').value;
        const shapeBorderColor = document.getElementById('shapeBorderColor').value;
        const shapeBorderWidth = document.getElementById('shapeBorderWidth').value;
        const shapeWidth = document.getElementById('shapeWidth').value;
        const shapeHeight = document.getElementById('shapeHeight').value;
        
        const currentSlideId = document.getElementById('currentSlideId').value;
        if (!currentSlideId) {
            alert('No slide selected');
            return;
        }

        const addBtn = this;
        const originalText = addBtn.innerHTML;
        addBtn.disabled = true;
        addBtn.innerHTML = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Adding...`;

        try {
            const response = await fetch('/api/elements/shape', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value
                },
                body: JSON.stringify({
                    slideId: currentSlideId,
                    shapeType: shapeType,
                    fillColor: shapeFillColor,
                    borderColor: shapeBorderColor,
                    borderWidth: parseInt(shapeBorderWidth),
                    width: parseInt(shapeWidth),
                    height: parseInt(shapeHeight)
                })
            });

            if (!response.ok) {
                throw new Error(await response.text() || 'Server error');
            }

            const element = await response.json();
            
            const slideCanvas = document.getElementById('slideCanvas');
            if (slideCanvas) {
                createShapeElement(element, slideCanvas);
            }
            
            const modal = bootstrap.Modal.getInstance(document.getElementById('addShapeModal'));
            modal?.hide();
        } catch (error) {
            console.error('Error:', error);
            alert('Error adding shape: ' + error.message);
        } finally {
            addBtn.disabled = false;
            addBtn.innerHTML = originalText;
        }
    });
}

function createShapeElement(elementData, parentElement) {
    const shapeElement = document.createElement('div');
    shapeElement.className = 'slide-element';
    shapeElement.dataset.elementId = elementData.id;
    shapeElement.style.cssText = `
        position: absolute;
        left: ${elementData.positionX}px;
        top: ${elementData.positionY}px;
        width: ${elementData.width}px;
        height: ${elementData.height}px;
        z-index: 10;
    `;
    
    let shapeStyle = '';
    const type = (elementData.type || '').toLowerCase();
    
    switch(type) {
        case 'circle':
            shapeStyle = `
                background-color: ${elementData.fillColor};
                border: ${elementData.borderWidth}px solid ${elementData.borderColor};
                width: 100%;
                height: 100%;
                box-sizing: border-box;
                border-radius: 50%;
            `;
            break;
        case 'triangle':
            shapeStyle = `
                width: 0;
                height: 0;
                border-left: ${elementData.width/2}px solid transparent;
                border-right: ${elementData.width/2}px solid transparent;
                border-bottom: ${elementData.height}px solid ${elementData.fillColor};
                border-top: none;
                background: none;
                position: absolute;
                top: 0;
                left: 0;
            `;
            break;
        default: 
            shapeStyle = `
                background-color: ${elementData.fillColor};
                border: ${elementData.borderWidth}px solid ${elementData.borderColor};
                width: 100%;
                height: 100%;
                box-sizing: border-box;
            `;
    }
    
    shapeElement.innerHTML = `<div style="${shapeStyle}"></div>`;
    parentElement.appendChild(shapeElement);
    
    shapeElement.addEventListener('mousedown', startDrag);
}



async function saveShapeStyle(element) {
    const elementId = element.dataset.elementId;
    if (!elementId) return;

    const shapeDiv = element.querySelector('div');
    if (!shapeDiv) return;

    try {
        const response = await fetch('/api/elements/shape/style', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value
            },
            body: JSON.stringify({
                elementId: elementId,
                fillColor: shapeDiv.style.backgroundColor || '#ffffff',
                borderColor: shapeDiv.style.borderColor || '#000000'
            })
        });

        if (!response.ok) {
            console.error('Failed to save shape style');
        }
    } catch (error) {
        console.error('Error saving shape style:', error);
    }
}
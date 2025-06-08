function initContextMenu() {
    const textContextMenu = document.getElementById('elementContextMenu');
    const shapeContextMenu = document.getElementById('shapeContextMenu');
    let currentElement = null;

    document.addEventListener('contextmenu', function(e) {
        const element = e.target.closest('.slide-element');
        if (element) {
            e.preventDefault();
            currentElement = element;
            
            const isText = element.querySelector('[contenteditable="true"]') !== null;
            
            if (isText) {
                textContextMenu.style.display = 'block';
                textContextMenu.style.left = `${e.pageX}px`;
                textContextMenu.style.top = `${e.pageY}px`;
                shapeContextMenu.style.display = 'none';
                
                const textDiv = element.querySelector('div');
                if (textDiv) {
                    const fontSize = parseInt(textDiv.style.fontSize) || 16;
                    document.getElementById('fontSizeSlider').value = fontSize;
                    
                    const fontStyle = textDiv.style.fontStyle || 'normal';
                    const fontWeight = textDiv.style.fontWeight || 'normal';
                    let styleValue = 'normal';
                    
                    if (fontWeight === 'bold' && fontStyle === 'italic') {
                        styleValue = 'bold italic';
                    } else if (fontWeight === 'bold') {
                        styleValue = 'bold';
                    } else if (fontStyle === 'italic') {
                        styleValue = 'italic';
                    }
                    
                    document.getElementById('fontStyleSelect').value = styleValue;
                }
            } else {
                shapeContextMenu.style.display = 'block';
                shapeContextMenu.style.left = `${e.pageX}px`;
                shapeContextMenu.style.top = `${e.pageY}px`;
                textContextMenu.style.display = 'none';
                
                const shapeDiv = element.querySelector('div');
                if (shapeDiv) {
                    document.getElementById('shapeFillColorPicker').value = 
                        rgbToHex(getComputedStyle(shapeDiv).backgroundColor) || '#ffffff';
                    document.getElementById('shapeBorderColorPicker').value = 
                        rgbToHex(getComputedStyle(shapeDiv).borderColor) || '#000000';
                }
            }
        }
    });

    document.addEventListener('click', function(e) {
        if (!textContextMenu.contains(e.target)) {
            textContextMenu.style.display = 'none';
        }
        if (!shapeContextMenu.contains(e.target)) {
            shapeContextMenu.style.display = 'none';
        }
    });

    document.getElementById('deleteElementBtn')?.addEventListener('click', async function() {
        if (currentElement) {
            const elementId = currentElement.dataset.elementId;
            if (elementId) {
                try {
                    const response = await fetch(`/api/elements/${elementId}`, {
                        method: 'DELETE',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value
                        },
                        body: JSON.stringify({ elementId })
                    });
                    
                    if (response.ok) {
                        currentElement.remove();
                    }
                } catch (error) {
                    console.error('Error deleting text:', error);
                }
            }
            textContextMenu.style.display = 'none';
        }
    });

    document.getElementById('bringTextToFrontBtn')?.addEventListener('click', function() {
        if (currentElement) bringToFront(currentElement);
        textContextMenu.style.display = 'none';
    });

    document.getElementById('deleteShapeBtn')?.addEventListener('click', async function() {
        if (currentElement) {
            const elementId = currentElement.dataset.elementId;
            if (elementId) {
                try {
                    const response = await fetch(`/api/elements/${elementId}`, {
                        method: 'DELETE',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value
                        },
                        body: JSON.stringify({ elementId })
                    });
                    
                    if (response.ok) {
                        currentElement.remove();
                    }
                } catch (error) {
                    console.error('Error deleting shape:', error);
                }
            }
            shapeContextMenu.style.display = 'none';
        }
    });

    document.getElementById('bringToFrontBtn')?.addEventListener('click', function() {
        if (currentElement) bringToFront(currentElement);
        shapeContextMenu.style.display = 'none';
    });

    // Text styling handlers
    document.getElementById('fontSizeSlider')?.addEventListener('input', function() {
        if (currentElement) {
            const textDiv = currentElement.querySelector('div[contenteditable="true"]');
            if (textDiv) {
                textDiv.style.fontSize = this.value + 'px';
                saveTextStyle(currentElement);
            }
        }
    });

    document.getElementById('fontStyleSelect')?.addEventListener('change', function() {
        if (currentElement) {
            const textDiv = currentElement.querySelector('div[contenteditable="true"]');
            if (textDiv) {
                const styleValue = this.value;
                textDiv.style.fontStyle = styleValue.includes('italic') ? 'italic' : 'normal';
                textDiv.style.fontWeight = styleValue.includes('bold') ? 'bold' : 'normal';
                saveTextStyle(currentElement);
            }
        }
    });

    document.getElementById('shapeFillColorPicker')?.addEventListener('change', function() {
        if (currentElement) {
            const shapeDiv = currentElement.querySelector('div');
            if (shapeDiv) {
                shapeDiv.style.backgroundColor = this.value;
                saveShapeStyle(currentElement);
            }
        }
    });

    document.getElementById('shapeBorderColorPicker')?.addEventListener('change', function() {
        if (currentElement) {
            const shapeDiv = currentElement.querySelector('div');
            if (shapeDiv) {
                shapeDiv.style.borderColor = this.value;
                saveShapeStyle(currentElement);
            }
        }
    });

    async function saveTextStyle(element) {
        const elementId = element.dataset.elementId;
        if (!elementId) return;

        const textDiv = element.querySelector('div[contenteditable="true"]');
        if (!textDiv) return;

        try {
            const response = await fetch('/api/elements/text/style', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value
                },
                body: JSON.stringify({
                    elementId: elementId,
                    fontSize: parseInt(textDiv.style.fontSize) || 16,
                    fontStyle: textDiv.style.fontStyle || 'normal',
                    fontWeight: textDiv.style.fontWeight || 'normal'
                })
            });

            if (!response.ok) {
                console.error('Failed to save text style');
            }
        } catch (error) {
            console.error('Error saving text style:', error);
        }
    }

    async function saveShapeStyle(element) {
        const elementId = element.dataset.elementId;
        if (!elementId) return;

        const shapeDiv = element.querySelector('div');
        if (!shapeDiv) return;

        try {
            const response = await fetch('/api/elements/shape/style', {
                method: 'PUT',
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

    function bringToFront(element) {
        const allElements = document.querySelectorAll('.slide-element');
        let maxZIndex = 0;
        
        allElements.forEach(el => {
            const zIndex = parseInt(el.style.zIndex) || 0;
            if (zIndex > maxZIndex) maxZIndex = zIndex;
        });
        
        element.style.zIndex = maxZIndex + 1;
        saveElementZIndex(element);
    }

    async function saveElementZIndex(element) {
        const elementId = element.dataset.elementId;
        if (!elementId) return;

        try {
            const response = await fetch('/api/elements/zindex', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value
                },
                body: JSON.stringify({
                    elementId: elementId,
                    zIndex: parseInt(element.style.zIndex) || 0
                })
            });

            if (!response.ok) {
                console.error('Failed to save z-index');
            }
        } catch (error) {
            console.error('Error saving z-index:', error);
        }
    }

    function rgbToHex(rgb) {
        if (!rgb || rgb === 'transparent') return '#ffffff';
        
        if (rgb.startsWith('#')) return rgb;
        
        const match = rgb.match(/^rgba?\((\d+),\s*(\d+),\s*(\d+)(?:,\s*\d+\.?\d*)?\)$/);
        if (!match) return '#ffffff';
        
        function hex(x) {
            return ("0" + parseInt(x).toString(16)).slice(-2);
        }
        
        return "#" + hex(match[1]) + hex(match[2]) + hex(match[3]);
    }
}
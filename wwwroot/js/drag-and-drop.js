
function initDragAndDrop() {
    document.querySelectorAll('.slide-element').forEach(element => {
        element.addEventListener('mousedown', startDrag);
    });
}

function startDrag(e) {
    if (e.button !== 0) return; 

    const element = e.target.closest('.slide-element');
    if (!element) return;

    e.preventDefault();
    e.stopPropagation();

    const startX = parseInt(element.style.left) || 0;
    const startY = parseInt(element.style.top) || 0;
    const startMouseX = e.clientX;
    const startMouseY = e.clientY;

    element.classList.add('dragging');

    function moveElement(e) {
        const deltaX = e.clientX - startMouseX;
        const deltaY = e.clientY - startMouseY;

        element.style.left = `${startX + deltaX}px`;
        element.style.top = `${startY + deltaY}px`;
    }

    function stopDrag() {
        document.removeEventListener('mousemove', moveElement);
        document.removeEventListener('mouseup', stopDrag);
        element.classList.remove('dragging');
        saveElementPosition(element);
    }

    document.addEventListener('mousemove', moveElement);
    document.addEventListener('mouseup', stopDrag, { once: true });
}

async function saveElementPosition(element) {
    const elementId = element.dataset.elementId;
    if (!elementId) return;

    try {
        const response = await fetch('/api/elements/position', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.getElementById('__RequestVerificationToken').value
            },
            body: JSON.stringify({
                elementId: elementId,
                positionX: parseInt(element.style.left),
                positionY: parseInt(element.style.top)
            })
        });

        if (!response.ok) {
            console.error('Failed to save position');
        }
    } catch (error) {
        console.error('Error saving position:', error);
    }
}
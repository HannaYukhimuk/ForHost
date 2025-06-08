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

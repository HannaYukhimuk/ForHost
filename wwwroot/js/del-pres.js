
        document.addEventListener('DOMContentLoaded', function() {
            const nicknameInput = document.getElementById('nicknameInput');
            
            // Handle join button clicks
            document.querySelectorAll('.join-btn').forEach(btn => {
                btn.addEventListener('click', function() {
                    const nickname = nicknameInput.value.trim();
                    if (!nickname) {
                        alert('Please enter your nickname');
                        return;
                    }
                    const presentationId = this.dataset.presentationId;
                    window.location.href = `/Home/Presentation/${presentationId}?nickname=${encodeURIComponent(nickname)}`;
                });
            });
            
            // Handle delete button clicks
            document.querySelectorAll('.delete-btn').forEach(btn => {
                btn.addEventListener('click', function() {
                    const nickname = nicknameInput.value.trim();
                    if (!nickname) {
                        alert('Please enter your nickname');
                        return;
                    }
                    
                    if (confirm('Are you sure you want to delete this presentation?')) {
                        const presentationId = this.dataset.presentationId;
                        const form = document.createElement('form');
                        form.method = 'post';
                        form.action = `/Home/Delete/${presentationId}`;
                        
                        const nicknameField = document.createElement('input');
                        nicknameField.type = 'hidden';
                        nicknameField.name = 'nickname';
                        nicknameField.value = nickname;
                        form.appendChild(nicknameField);
                        
                        document.body.appendChild(form);
                        form.submit();
                    }
                });
            });
        });
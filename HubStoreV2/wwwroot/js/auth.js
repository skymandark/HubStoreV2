// Authentication JavaScript Helper Functions
class AuthHelper {
    constructor() {
        this.baseApiUrl = '/api/auth';
        this.init();
    }

    init() {
        // Check authentication status on page load
        this.checkAuthStatus();
        
        // Setup auto-logout timer
        this.setupAutoLogout();
    }

    // Check if user is authenticated
    async checkAuthStatus() {
        try {
            const response = await fetch(`${this.baseApiUrl}/current-user`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${this.getToken()}`
                }
            });

            if (response.ok) {
                const user = await response.json();
                this.updateUserUI(user);
                return true;
            } else {
                this.clearAuthData();
                return false;
            }
        } catch (error) {
            console.error('Auth check failed:', error);
            this.clearAuthData();
            return false;
        }
    }

    // Login with email and password
    async login(email, password, rememberMe = false) {
        try {
            const response = await fetch(`${this.baseApiUrl}/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    email: email,
                    password: password,
                    rememberMe: rememberMe
                })
            });

            const result = await response.json();

            if (response.ok && result.succeeded) {
                this.setAuthData(result.user, rememberMe);
                return { success: true, user: result.user };
            } else {
                return { success: false, message: result.message || 'Login failed' };
            }
        } catch (error) {
            console.error('Login error:', error);
            return { success: false, message: 'Network error occurred' };
        }
    }

    // Login with PIN code
    async loginWithPin(pinCode) {
        try {
            const response = await fetch(`${this.baseApiUrl}/login-with-pin`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    pinCode: pinCode
                })
            });

            const result = await response.json();

            if (response.ok && result.succeeded) {
                this.setAuthData(result.user, false);
                return { success: true, user: result.user };
            } else {
                return { success: false, message: result.message || 'PIN login failed' };
            }
        } catch (error) {
            console.error('PIN login error:', error);
            return { success: false, message: 'Network error occurred' };
        }
    }

    // Register new user
    async register(userData) {
        try {
            const response = await fetch(`${this.baseApiUrl}/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(userData)
            });

            const result = await response.json();

            if (response.ok && result.succeeded) {
                return { success: true, user: result.user };
            } else {
                return { 
                    success: false, 
                    message: result.message || 'Registration failed',
                    errors: result.errors || []
                };
            }
        } catch (error) {
            console.error('Registration error:', error);
            return { success: false, message: 'Network error occurred' };
        }
    }

    // Logout user
    async logout() {
        try {
            await fetch(`${this.baseApiUrl}/logout`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${this.getToken()}`
                }
            });
        } catch (error) {
            console.error('Logout error:', error);
        } finally {
            this.clearAuthData();
            window.location.href = '/Auth/Login';
        }
    }

    // Get current user
    async getCurrentUser() {
        try {
            const response = await fetch(`${this.baseApiUrl}/current-user`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${this.getToken()}`
                }
            });

            if (response.ok) {
                return await response.json();
            } else {
                return null;
            }
        } catch (error) {
            console.error('Get current user error:', error);
            return null;
        }
    }

    // Store authentication data
    setAuthData(user, rememberMe = false) {
        const storage = rememberMe ? localStorage : sessionStorage;
        storage.setItem('authUser', JSON.stringify(user));
        storage.setItem('authToken', user.token || '');
        storage.setItem('authTimestamp', Date.now().toString());
    }

    // Clear authentication data
    clearAuthData() {
        localStorage.removeItem('authUser');
        localStorage.removeItem('authToken');
        localStorage.removeItem('authTimestamp');
        sessionStorage.removeItem('authUser');
        sessionStorage.removeItem('authToken');
        sessionStorage.removeItem('authTimestamp');
    }

    // Get token
    getToken() {
        return localStorage.getItem('authToken') || sessionStorage.getItem('authToken') || '';
    }

    // Get current user from storage
    getStoredUser() {
        const userStr = localStorage.getItem('authUser') || sessionStorage.getItem('authUser');
        return userStr ? JSON.parse(userStr) : null;
    }

    // Check if user is admin
    isAdmin() {
        const user = this.getStoredUser();
        return user && user.isAdmin;
    }

    // Update UI with user information
    updateUserUI(user) {
        // Update user info in header
        const userInfoElements = document.querySelectorAll('.user-info span');
        userInfoElements.forEach(element => {
            element.textContent = user.userName || user.email;
        });

        // Update user avatar
        const avatarElements = document.querySelectorAll('.user-avatar');
        avatarElements.forEach(element => {
            element.innerHTML = `<i class="fas fa-user"></i>`;
            if (user.userName) {
                element.textContent = user.userName.charAt(0).toUpperCase();
            }
        });

        // Show/hide admin-only elements
        const adminElements = document.querySelectorAll('[data-admin-only]');
        adminElements.forEach(element => {
            element.style.display = user.isAdmin ? '' : 'none';
        });
    }

    // Setup auto-logout timer
    setupAutoLogout() {
        const checkInterval = 60000; // Check every minute
        const maxAge = 8 * 60 * 60 * 1000; // 8 hours

        setInterval(() => {
            const timestamp = localStorage.getItem('authTimestamp') || sessionStorage.getItem('authTimestamp');
            if (timestamp && (Date.now() - parseInt(timestamp)) > maxAge) {
                this.logout();
            }
        }, checkInterval);
    }

    // Show alert message
    showAlert(message, type = 'info', container = '#alertContainer') {
        const alertContainer = document.querySelector(container);
        if (!alertContainer) return;

        const alertHtml = `
            <div class="alert alert-${type} alert-dismissible fade show animate__animated animate__fadeIn" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `;
        
        alertContainer.innerHTML = alertHtml;

        // Auto dismiss after 5 seconds
        setTimeout(() => {
            const alert = alertContainer.querySelector('.alert');
            if (alert) {
                alert.classList.remove('show');
                setTimeout(() => alert.remove(), 150);
            }
        }, 5000);
    }

    // Validate email format
    isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    // Check password strength
    checkPasswordStrength(password) {
        let strength = 0;
        const feedback = [];

        if (password.length >= 8) {
            strength++;
        } else {
            feedback.push('8 characters minimum');
        }

        if (password.match(/[a-z]/) && password.match(/[A-Z]/)) {
            strength++;
        } else {
            feedback.push('Upper and lower case letters');
        }

        if (password.match(/[0-9]/)) {
            strength++;
        } else {
            feedback.push('At least one number');
        }

        if (password.match(/[^a-zA-Z0-9]/)) {
            strength++;
        } else {
            feedback.push('At least one special character');
        }

        const levels = ['Weak', 'Fair', 'Good', 'Strong'];
        return {
            score: strength,
            level: levels[Math.min(strength - 1, 3)],
            feedback: feedback
        };
    }

    // Format phone number
    formatPhoneNumber(phone) {
        // Remove all non-numeric characters
        const cleaned = phone.replace(/\D/g, '');
        
        // Format for Saudi numbers (05xxxxxxxx)
        if (cleaned.length === 10 && cleaned.startsWith('05')) {
            return cleaned;
        }
        
        return cleaned;
    }
}

// Initialize auth helper
window.authHelper = new AuthHelper();

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = AuthHelper;
}

import React, { useState, useEffect } from "react";

const COOKIE_NAME = "cookieConsent";
const COOKIE_EXPIRY_DAYS = 365;

const CookieConsent: React.FC = () => {
    const [isVisible, setIsVisible] = useState<boolean>(false);

    useEffect(() => {
        const consent = localStorage.getItem(COOKIE_NAME);
        if (!consent) {
            setIsVisible(true);
        }
    }, []);

    const handleAccept = () => {
        setConsent(true);
        setIsVisible(false);
    };

    const handleDecline = () => {
        setConsent(false);
        setIsVisible(false);
    };

    const setConsent = (consent: boolean) => {
        localStorage.setItem(COOKIE_NAME, JSON.stringify({ consent, date: new Date() }));
    };

    if (!isVisible) return null;

    return (
        <div style={{ position: 'absolute', bottom: 0, width:'100%' }} className="cookie-consent fixed bottom-0 left-0 w-full bg-gray-800 text-white p-4 flex flex-col md:flex-row items-center justify-between shadow-lg">
            <div className="message mb-3 md:mb-0">
                <p className="text-sm">
                    We use cookies to improve your experience. By using this site, you agree to our use of cookies. Learn more in our{" "}
                    <a href="/privacy-policy" className="underline text-blue-400 hover:text-blue-300">
                        Privacy Policy
                    </a>.
                </p>
            </div>
            <div className="actions flex space-x-3">
                <button
                    onClick={handleAccept}
                    className="bg-theme px-4 py-2 rounded-md hover:bg-blue-500 transition"
                >
                    Accept
                </button>
                <button
                    onClick={handleDecline}
                    className="bg-light px-4 py-2 rounded-md hover:bg-gray-600 transition"
                >
                    Decline
                </button>
            </div>
        </div>
    );
};

export default CookieConsent;

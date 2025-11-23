import React, { useEffect, useRef } from 'react';

interface FormErrorAlertProps {
  errors?: string[];
  Errors?: string[];
}

const FormErrorAlert: React.FC<FormErrorAlertProps> = ({ errors, Errors }) => {
  const allErrors = errors || Errors;
  const alertRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (allErrors && allErrors.length > 0 && alertRef.current) {
      // Scroll into view and focus for accessibility
      alertRef.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
      alertRef.current.focus();
    }
  }, [allErrors]);

  if (!allErrors || allErrors.length === 0) return null;

  return (
    <div
      ref={alertRef}
      tabIndex={-1} // makes the div focusable
      className="mb-lg-15 alert alert-danger outline-none"
      aria-live="assertive"
      role="alert"
    >
      <div className="alert-text font-weight-bold">
        <ul className="list-disc list-inside">
          {allErrors?.map((err, index) => (
            <li key={index}>{err}</li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default FormErrorAlert;

import { useEffect, useRef } from "react";
import React, { useState } from "react";
import { toAbsoluteUrl } from "../../../helpers";
import { AUTH_LOCAL_STORAGE_KEY } from "../../../../app/modules/auth";
import {
  getSelectedCountryCode,
  setSelectedCountryCode,
} from "../../../helpers/AppUtil";
import AuthService from "../../../../services/AuthService";
import PermissionService from "../../../../services/PermissionService";

interface Country {
  id: string;
  countryName: string;
  code: string;
  currencyName: string;
  currencyPrefix?: string;
  currencySuffix?: string;
  isActive?: boolean;
}
const permissionService = new PermissionService();
const SELECTED_COUNTRY_CODE_KEY = "selected_country_code";

const CountryDropdown: React.FC = () => {
  const [countries, setCountries] = useState<Country[]>([]);
  const [selectedCode, setSelectedCode] = useState(
    () => getSelectedCountryCode() || "KE"
  );
  const [loading, setLoading] = useState(false);
  const [isOpen, setIsOpen] = useState(false);

  const dropdownRef = useRef<HTMLDivElement>(null);

  // Load countries from localStorage on mount
  useEffect(() => {
    const stored = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        const userCountries = parsed.countries ?? [];
        setCountries(userCountries);
        const defaultCode =
          userCountries.find((c: Country) => c.isActive)?.code ??
          userCountries[0]?.code;
        setSelectedCode(defaultCode);
      } catch (err) {
        console.error("Failed to parse countries from localStorage:", err);
      }
    }
  }, []);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(event.target as Node)
      ) {
        setIsOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  useEffect(() => {
    const stored = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        const userCountries = parsed.countries ?? [];
        setCountries(userCountries);

        const savedCode = getSelectedCountryCode();
        const defaultCode =
          userCountries.find((c: Country) => c.isActive)?.code ??
          userCountries[0]?.code;

        const isSavedCodeValid = userCountries.some(
          (c: Country) => c.code === savedCode
        );

        const codeToUse = isSavedCodeValid ? savedCode : defaultCode || "KE";

        setSelectedCode(codeToUse);
        setSelectedCountryCode(codeToUse); // Ensure it's stored if not already
      } catch (err) {
        console.error("Failed to parse countries from localStorage:", err);
      }
    }
  }, []);

  const updatePermissionsForCountry = async () => {
    setLoading(true); // show loader or message

    const stored = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);

    if (stored) {
      try {
        const parsed = JSON.parse(stored);

        let per = await permissionService.GetPermissions(parsed.username);

        parsed.permissions = per;
        localStorage.setItem(AUTH_LOCAL_STORAGE_KEY, JSON.stringify(parsed));

        setLoading(false); // hide loader before reload
        window.location.reload();
      } catch (err) {
        setLoading(false);
        console.error("Failed to update permissions in localStorage:", err);
        alert("Permission update failed. Please try again.");
      }
    } else {
      setLoading(false);
    }
  };

  const currentCountry = countries.find((c) => c.code === selectedCode);

  if (countries.length === 0) return null; // Early return if no countries

  return (
    <div className="dropdown" ref={dropdownRef}>
      <button
        className="btn btn-light dropdown-toggle d-flex align-items-center"
        type="button"
        onClick={() => setIsOpen(!isOpen)}
        aria-expanded={isOpen}
      >
        <img
          src={toAbsoluteUrl(
            `media/flags/${
              currentCountry?.countryName?.toLowerCase() || "kenya"
            }.svg`
          )}
          alt={currentCountry?.countryName || "Kenya"}
          width="20"
          className="me-2"
        />
        {currentCountry?.countryName || "Kenya"}
      </button>

      {isOpen && (
        <ul className="dropdown-menu show">
          {countries.map((c) => (
            <li key={c.code}>
              <button
                className={`dropdown-item d-flex align-items-center ${
                  c.code === selectedCode ? "active" : ""
                }`}
                onClick={() => {
                  setSelectedCode(c.code);
                  setSelectedCountryCode(c.code);
                  setIsOpen(false);
                  updatePermissionsForCountry();
                }}
              >
                <img
                  src={toAbsoluteUrl(
                    `media/flags/${c.countryName?.toLowerCase() || "kenya"}.svg`
                  )}
                  alt={c.countryName || "Kenya"}
                  width="20"
                  className="me-2"
                />
                {c.countryName}
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default CountryDropdown;

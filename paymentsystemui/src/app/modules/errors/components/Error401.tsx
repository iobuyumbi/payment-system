import { FC } from "react";
import { Link } from "react-router-dom";
import { KTCard, KTCardBody, toAbsoluteUrl } from "../../../../_metronic/helpers";
import { Content } from "../../../../_metronic/layout/components/content";

const Error401: FC = () => {
  return (
    <>
      <Content>
        <KTCard className="shadow mt-10">
          <KTCardBody className="d-flex shadow h-500px justify-content-center align-items-center">
          <div className="d-flex flex-column justify-content-center align-items-center  text-center">
            <h1 className="fw-bolder fs-2hx text-gray-900 mb-4">Oops!</h1>
            <div className="fw-semibold fs-6 text-gray-500 mb-7">
              You're not authorised to view this page.
            </div>
            <div className="mb-3">
              <h1 className="mw-100 mh-300px theme-light-show  text-danger">
                Error 401
              </h1>
              <h1 className="mw-100 mh-300px theme-dark-show text-danger">
                Error 401
              </h1>
            </div>
            <div className="mb-0">
              <Link to="/dashboard" className="btn btn-sm btn-primary">
                Return Home
              </Link>
            </div>
          </div>{" "}</KTCardBody>
        </KTCard>
      </Content>
    </>
  );
};

export { Error401 };

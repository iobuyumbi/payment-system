import React, { useState } from "react";
import { data } from "../../../_shared/data/loanUI.json";
import { Link } from "react-router-dom";
import { Content } from "../../../_metronic/layout/components/content";

export default function LoansIndex() {
  const [reportsArr, setReportArr] = useState(data);

  return (
    <Content>
      <div className="row">
        {reportsArr &&
          reportsArr.map((item: any, index: any) => (
            <div className="col-md-3" key={index}>
              <div className="card mb-xl-5">
                <div className="card-header border mb-0">
                  <div className="card-title m-0">
                    <h3 className="fw-bolder m-0">{item.title}</h3>
                  </div>
                </div>

                <div className="card-body">
                  <div className="d-flex row">
                    <div className="">
                      <div className="fw-normal">
                        <div className="fs-5 text-gray-600 px-3">
                          {item.description}
                        </div>
                      </div>
                    </div>
                    <div className="text-center mt-10">
                      <Link
                        className="btn btn-secondary fs-4 w-100"
                        to={item.link}
                      >
                        View
                      </Link>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}
      </div>
    </Content>
  );
}

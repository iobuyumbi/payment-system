import { useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import Level1Add from "./cards/Level1Add";
import AdminLevel1List from "./cards/Level1List";
import { Error401 } from "../errors/components/Error401";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";

export function AdminLevel1() {
  const [countryId, setCountryId] = useState<any>();
  const [countyData, setCountyData] = useState<any>();
  return (
    <>
      {isAllowed("settings.administrative.view") ? (
        <Content>
          {isAllowed("settings.administrative.add") ? (
            <Level1Add
              countryId={countryId}
              countyData={countyData}
              setCountryId={setCountryId}
            />
          ) : (
            ""
          )}
          <AdminLevel1List
            setCountyData={setCountyData}
            countryId={countryId}
          />
        </Content>
      ) : (
        <Error401 />
      )}
    </>
  );
}

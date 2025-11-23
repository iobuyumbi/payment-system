import { Content } from "../../../_metronic/layout/components/content";
import AdminLevel2List from "./cards/Level2List";
import Level2Add from "./cards/Level2Add";
import { useState } from "react";
import { Error401 } from "../errors/components/Error401";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";

export function AdminLevel2() {
  const [countyId, setCountyId] = useState<any>();
  const [subCountyData, setSubCountyData] = useState<any>();
  const [countryId, setCountryId] = useState<any>();
  return (
    <>
      {isAllowed("settings.administrative.view") ? (
        <Content>
          {isAllowed("settings.administrative.add") ? (
            <Level2Add
              countyId={countyId}
              subCountyData={subCountyData}
              setCountyId={setCountyId}
            />
          ) : (
            ""
          )}
          <AdminLevel2List
            setSubCountyData={setSubCountyData}
            countyId={countyId}
          />
        </Content>
      ) : (
        <Error401 />
      )}
    </>
  );
}

import { useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import Level3Add from "./cards/Level3Add";
import AdminLevel3List from "./cards/Level3List";
import { Error401 } from "../errors/components/Error401";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";

export function AdminLevel3() {
  const [subCountyId, setSubCountyId] = useState<any>();
  const [wardData, setWardData] = useState<any>();
  return (
    <>
      {" "}
      {isAllowed("settings.administrative.view") ? (
        <Content>
          {isAllowed("settings.administrative.add") ? (
            <Level3Add
              subCountyId={subCountyId}
              wardData={wardData}
              setSubCountyId={setSubCountyId}
            />
          ) : (
            ""
          )}
          <AdminLevel3List
            setWardData={setWardData}
            subCountyId={subCountyId}
          />
        </Content>
      ) : (
        <Error401 />
      )}
    </>
  );
}

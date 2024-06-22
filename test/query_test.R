require(Darwinism);

imports "memory_query" from "Darwinism";

let test_data = read.csv(file.path(@dir, "..", "proj_stats.csv"), row.names = NULL);

test_data[, "proj_name"] = basename(test_data$proj);
test_data[, "filename"] = basename(test_data$files);

print("get input data table:");
print(test_data, max.print = 13);

let table = memory_query::load(test_data)
|> fulltext(["proj","files"])
|> hashindex(["proj_name", "filename"])
|> valueindex(totalLines = "integer", lineOfCodes = "integer")
;

table 
|> select(proj_name = "Docker.NET5", "totalLines" |> between([30,80]))
|> print()
;

table
|> select(lineOfCodes > 100, match_against("proj","google"))
|> print()
;
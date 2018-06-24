DELETE FROM [dbo].[OrgListItem];
DELETE FROM [dbo].[OrgList];

-- Grocery List
INSERT INTO [dbo].[OrgList]
VALUES ('92CD602E-A7B8-4E15-AB7C-BFC71B43EBF3', 'Groceries', 'A list of grocery items to get from the store.');

INSERT INTO [dbo].[OrgListItem]
VALUES ('EDCBB0D8-7C1A-4B7F-98C3-4DB7699E5D77', '92CD602E-A7B8-4E15-AB7C-BFC71B43EBF3', 'Milk', 0);

INSERT INTO [dbo].[OrgListItem]
VALUES ('E70808AD-A488-4F21-B0FB-A27A35BF0EF1', '92CD602E-A7B8-4E15-AB7C-BFC71B43EBF3', 'Eggs', 0);

-- To Do Items
INSERT INTO [dbo].[OrgList]
VALUES ('7D5B8F14-93AB-4C5C-8DE9-62AE3351BFBF', 'To Do', 'A list of tasks that need to be completed.');

